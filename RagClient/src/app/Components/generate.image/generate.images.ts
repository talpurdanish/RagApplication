import { Component, OnInit, signal } from '@angular/core';
import { ImageService } from '../../BussinessLogic/Services/Image.Service';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
  createGeneratedImageModel,
  GeneratedImageModel,
} from '../../BussinessLogic/Models/Image.Model';
import {
  lucideArrowLeft,
  lucideArrowRight,
  lucideRefreshCcw,
  lucideSearch,
  lucideX,
} from '@ng-icons/lucide';
import { Progress } from '../../Layouts/MainLayout/progress/progress';

@Component({
  selector: 'app-generate-images',
  imports: [NgIcon, Progress],
  viewProviders: [
    provideIcons({ lucideRefreshCcw, lucideX, lucideArrowLeft, lucideArrowRight, lucideSearch }),
  ],
  templateUrl: './generate.images.html',
  styleUrl: './generate.images.css',
})
export class GenerateImagesComponent implements OnInit {
  list = signal<GeneratedImageModel[]>([]);

  models = [
    '@cf/blackforestlabs/ux-1-schnell',
    '@cf/bytedance/stable-diffusion-xl-lightning',
    '@cf/lykon/dreamshaper-8-lcm',
    '@cf/runwayml/stable-diffusion-v1-5-img2img',
    '@cf/runwayml/stable-diffusion-v1-5-inpainting',
    '@cf/stabilityai/stable-diffusion-xl-base-1.0',
  ];

  selectedModel = signal<string>(this.models[0]);

  onSelectChange(event: Event) {
    const model = (event.target as HTMLSelectElement).value;
    this.selectedModel.set(model);
  }

  loading = signal<boolean>(false);
  imageUrl = signal<string | null>(null);
  imageResponseRecieved = signal<boolean>(false);

  searchText = signal<string>('');

  constructor(private imageService: ImageService) {}

  ngOnInit(): void {}
  generateImage() {
    if (this.searchText() != '' && this.selectedModel() != '') {
      this.loading.set(true);
      this.imageResponseRecieved.set(false);
      this.imageService.GenerateImage(this.searchText(), this.selectedModel())?.subscribe({
        next: (data) => {
          this.imageResponseRecieved.set(true);
          this.imageUrl.set(URL.createObjectURL(data));
          this.loading.set(false);
        },
        error: (err) => {
          this.imageResponseRecieved.set(false);
          console.error(err);
          this.imageUrl.set('');
          this.loading.set(false);
        },
      });
    }
  }

  inputChanged($event: HTMLInputElement) {
    this.searchText.set((event?.target as HTMLInputElement).value);
    this.loading.set(false);
    this.imageResponseRecieved.set(false);
  }

  reset() {
    this.loading.set(false);
    this.imageResponseRecieved.set(false);
  }
}
