import { Component, OnInit, signal } from '@angular/core';
import { form, FormRoot } from '@angular/forms/signals';
import { DocumentService } from '../../BussinessLogic/Services/Document.Service';
import { ApiResponse } from '../../BussinessLogic/Models/Generics/ApiResponse';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideRefreshCcw, lucideX } from '@ng-icons/lucide';

import { createDocumentModel, DocumentModel } from '../../BussinessLogic/Models/Document.Model';
import { StorageService } from '../../BussinessLogic/Services/Storage.Service';
import { Progress } from "../../Layouts/MainLayout/progress/progress";

interface UploadData {
  file: File | null;
}

@Component({
  selector: 'app-document',
  imports: [FormRoot, NgIcon, Progress],
  providers: [DocumentService, NgIcon],
  viewProviders: [provideIcons({ lucideRefreshCcw, lucideX })],
  templateUrl: './document.html',
  styleUrl: './document.css',
})
export class DocumentComponent implements OnInit {
  reply: string = '';
  uploadModel = signal<UploadData>({
    file: null,
  });

  list = signal<DocumentModel[]>([]);
  inProcess = signal<boolean>(false);

  constructor(
    private documentService: DocumentService,
    private storageService: StorageService,
  ) {}

  ngOnInit(): void {
    this.loadDocuments();
  }

  loadDocuments() {
    this.inProcess.set(true);
    this.documentService.GetAll()?.subscribe({
      next: (data) => {
        if (data != undefined && data != null) {
          var res: ApiResponse<DocumentModel> = new ApiResponse<DocumentModel>(data);
          if (
            res != undefined &&
            res != null &&
            res.isSuccess() &&
            res.results != undefined &&
            res.results != null
          ) {
            var iList = res.results.map((d) => createDocumentModel(d));
            this.list.set(iList);
            this.inProcess.set(false);
          }
        }
      },
      error: () => {
        this.inProcess.set(false);
      },
    });
  }

  startAnalysis() {
    this.documentService.StartAnalysis().subscribe();
  }

  uploadForm = form(this.uploadModel, {
    submission: {
      action: async (field) => {
        this.inProcess.set(true);
        const payload = new FormData();
        if (field.file().value() != null) {
          const inputFile = field.file().value();
          if (inputFile != undefined) payload.append('file', inputFile);
        }
        this.documentService.InitDocument(payload)?.subscribe({
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
                alert('Embeddings have been updated');
                this.inProcess.set(false);
              }
            }
          },
          error: () => {
            this.inProcess.set(false);
          },
        });

        return { kind: 'serverError', message: 'Failed to submit form' };
      },
    },
  });

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.uploadModel.update((val) => ({ ...val, file: input.files![0] }));
    }
  }
}
