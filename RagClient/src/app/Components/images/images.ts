import { Component, ElementRef, OnInit, signal, ViewChild } from '@angular/core';
import { form, FormField, FormRoot } from '@angular/forms/signals';
import { ImageService } from '../../BussinessLogic/Services/Image.Service';
import { ApiResponse } from '../../BussinessLogic/Models/Generics/ApiResponse';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { createImageModel, ImageModel } from '../../BussinessLogic/Models/Image.Model';
import {
  lucideArrowLeft,
  lucideArrowRight,
  lucideEye,
  lucideInfo,
  lucideRefreshCcw,
  lucideSearch,
  lucideX,
} from '@ng-icons/lucide';
import { DecimalPipe } from '@angular/common';
import { Progress } from '../../Layouts/MainLayout/progress/progress';

interface MultiUploadModel {
  files: File[];
}

interface UploadModel {
  file: File | null;
}

@Component({
  selector: 'app-images',
  imports: [FormRoot, NgIcon, DecimalPipe, Progress],
  providers: [NgIcon],
  viewProviders: [
    provideIcons({
      lucideRefreshCcw,
      lucideX,
      lucideArrowLeft,
      lucideArrowRight,
      lucideSearch,
      lucideEye,
      lucideInfo,
    }),
  ],
  templateUrl: './images.html',
  styleUrl: './images.css',
})
export class ImagesComponent implements OnInit {
  multiInProcess = signal<boolean>(false);
  singleInProcess = signal<boolean>(false);
  fileSelected = signal<boolean>(false);
  imageClicked = signal<boolean>(false);
  selectedImage = signal<string>('');
  selectedAiInsight = signal<string | null>(null);
  selectedImageWidth = signal<number>(0);
  selectedImageHeight = signal<number>(0);

  multiModel = signal<MultiUploadModel>({ files: [] });
  uploadModel = signal<UploadModel>({ file: null });

  rImage = signal<string>('');
  rAiInsight = signal<string | null>(null);
  rImageWidth = signal<number>(0);
  rImageHeight = signal<number>(0);
  imageReturned = signal<boolean>(false);

  list = signal<ImageModel[]>([]);

  searched = signal<boolean>(false);
  searchInProgress = signal<boolean>(false);
  loading = signal<boolean>(false);

  constructor(private imageService: ImageService) {}

  ngOnInit(): void {
    this.loadImages();
  }
  loadImages() {
    this.loading.set(true);
    this.imageService.GetAll()?.subscribe({
      next: (data) => {
        if (!data.error && data.results) {
          const iList = data.results.map((raw: any) => createImageModel(raw));
          iList.forEach((image) => {
            image.image = this.convertToImage(image.image, image.format);
            image.thumbnailImage = this.convertToImage(image.thumbnailImage, image.thumbFormat);
          });
          this.list.set(iList);
          this.searched.set(false);
          this.loading.set(false);
        }
      },
      error: () => {
        this.list.set([]);
        this.searched.set(false);
        this.loading.set(false);
      },
    });
  }

  convertToImage(binary: any, imageType: string): any {
    var slashIndex = binary.indexOf('base64');
    var base64 = true;
    if (slashIndex == -1) {
      slashIndex = binary.indexOf('/');
      base64 = false;
    }
    const newIndex = slashIndex + (base64 ? 6 : 0);
    const newBinary = binary.substring(newIndex, binary.length - newIndex);

    const str = 'data:' + imageType + ';base64,' + newBinary;

    return str;
  }

  startAnalysis() {
    this.imageService.StartAnalysis().subscribe();
  }

  insight = signal<string>('');

  getInsight(image: ImageModel) {
    image.inProgress = true;
    this.imageService.GetInsight(image.id).subscribe({
      next: (data) => {
        if (data != undefined && data != null) {
          var res: ApiResponse<string> = new ApiResponse<string>(data);
          if (
            res != undefined &&
            res != null &&
            res.isSuccess() &&
            res.result != undefined &&
            res.result != null
          ) {
            image.inProgress = false;
            this.insight.set(res.result);
          }
        }
      },
      error: (err) => {
        this.insight.set(err);
      },
    });
  }

  onFileSelected(event: any, single: boolean = false) {
    if (event.target.files && event.target.files.length) {
      this.fileSelected.set(true);
      if (single) {
        this.uploadModel.update((val) => ({ ...val, file: event.target.files![0] }));
      } else {
        this.multiModel.update((val) => ({ ...val, files: event.target.files }));
      }
    }
  }

  multiUploadForm = form(this.multiModel, {
    submission: {
      action: async (field) => {
        const payload = new FormData();
        var inputFiles = field.files().value();
        alert(`${this.fileSelected()}, ${inputFiles != null}, ${inputFiles.length}`);
        if (this.fileSelected() && inputFiles != null && inputFiles.length > 0) {
          this.fileSelected.set(false);
          this.multiInProcess.set(true);
          Array.from(inputFiles).forEach((file) => {
            if (file != undefined) {
              payload.append('files', file);
            }
          });

          this.imageService.MultiUploadImage(payload)?.subscribe({
            next: (data) => {
              if (data != undefined && data != null) {
                var res: ApiResponse<boolean> = new ApiResponse<boolean>(data);
                if (
                  res != undefined &&
                  res != null &&
                  res.isSuccess() &&
                  res.result != undefined &&
                  res.result != null
                ) {
                  alert('Images have been uploaded');
                  this.multiInProcess.set(false);
                  this.loadImages();
                }
              }
            },
            error: () => {
              this.multiInProcess.set(false);
            },
          });
        }
        return { kind: 'serverError', message: 'Failed to submit form' };
      },
    },
  });

  uploadForm = form(this.uploadModel, {
    submission: {
      action: async (field) => {
        this.imageReturned.set(false);
        const payload = new FormData();
        const inputFile = field.file().value();
        if (this.fileSelected() && inputFile != null) {
          this.fileSelected.set(false);
          this.singleInProcess.set(true);
          if (inputFile != undefined) payload.append('file', inputFile);
          this.imageService.UploadImage(payload)?.subscribe({
            next: (data) => {
              if (data != undefined && data != null) {
                var res: ApiResponse<ImageModel> = new ApiResponse<ImageModel>(data);

                if (
                  res != undefined &&
                  res != null &&
                  res.isSuccess() &&
                  res.result != undefined &&
                  res.result != null
                ) {
                  alert('Image has been uploaded');
                  this.loadImages();
                  this.singleInProcess.set(false);
                }
              }
            },
            error: () => {
              this.singleInProcess.set(false);
            },
          });
        }
        return { kind: 'serverError', message: 'Failed to submit form' };
      },
    },
  });

  hasNext = signal<boolean>(true);
  hasPrev = signal<boolean>(false);
  currentIndex = signal<number>(0);
  total = signal<number>(0);

  showImage(image: ImageModel, index: number) {
    this.updateNavigation(index);
    this.selectedAiInsight.set(image.aiInsights);
    this.selectedImage.set(image.image);
    this.selectedImageWidth.set(image.width);
    this.selectedImageHeight.set(image.height);
    this.imageReturned.set(true);
    this.imageClicked.set(true);
  }

  updateNavigation(index: number) {
    this.total.set(this.list().length);
    this.hasNext.set(index < this.total() - 1);
    this.hasPrev.set(index > 0);
    this.currentIndex.set(index);
  }

  showNext() {
    if (this.hasNext()) {
      var newIndex = this.currentIndex() + 1;
      this.showImage(this.list()[newIndex], newIndex);
      this.updateNavigation(newIndex);
    }
  }

  showPrev() {
    if (this.hasPrev()) {
      var newIndex = this.currentIndex() - 1;
      this.showImage(this.list()[newIndex], newIndex);
      this.updateNavigation(newIndex);
      this.updateNavigation(newIndex);
    }
  }

  closeImage() {
    this.selectedAiInsight.set('');
    this.selectedImage.set('');
    this.selectedImageWidth.set(-1);
    this.selectedImageHeight.set(-1);
    this.imageClicked.set(false);
  }

  inputChanged() {
    this.searchInProgress.set(false);
  }

  reset() {
    this.searchInProgress.set(false);
    this.loadImages();
  }

  search(query: string) {
    this.searchInProgress.set(true);
    this.imageService.Search(query)?.subscribe({
      next: (data) => {
        if (!data.error && data.results) {
          const iList = data.results.map((raw: any) => createImageModel(raw));
          iList.forEach((image) => {
            image.image = this.convertToImage(image.image, image.format);
            image.thumbnailImage = this.convertToImage(image.thumbnailImage, image.thumbFormat);
          });
          this.list.set(iList);
          this.searchInProgress.set(false);
          this.searched.set(true);
          this.updateNavigation(0);
        }
      },
      error: () => {
        this.list.set([]);

        this.searchInProgress.set(false);
        this.searched.set(true);
        this.updateNavigation(0);
      },
    });
  }
}
