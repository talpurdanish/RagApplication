// api/UserService.ts
import { Injectable } from '@angular/core';

import { ApiResponse } from '../Models/Generics/ApiResponse';
import { HttpApiClient } from './Generics/HttpApiClient';
import { Observable } from 'rxjs';
import { ImageModel } from '../Models/Image.Model';

const baseUri = '/images';
@Injectable({ providedIn: 'root' })
export class ImageService {
  constructor(private client: HttpApiClient) {}

  Get(id: number): Observable<ApiResponse<ImageModel>> {
    const url = `${baseUri}/GetImage/${id}`;
    const res = this.client.get<ImageModel>(url, null);
    return res;
  }

  GetInsight(id: number): Observable<ApiResponse<string>> {
    const url = `${baseUri}/GetImageInsight/${id}`;
    const res = this.client.get<string>(url, null);
    return res;
  }

  StartAnalysis(): Observable<ApiResponse<boolean>> {
    const url = `${baseUri}/StartAnalysis`;
    const res = this.client.get<boolean>(url, null);
    return res;
  }

  GetAll(): Observable<ApiResponse<ImageModel>> | null {
    const url = `${baseUri}/GetImages`;
    const res = this.client.get<ImageModel>(url, null);
    return res;
  }

  GenerateImage(query: string, model: string): Observable<Blob> {
    var m = model == '' ? '@cf/stabilityai/stable-diffusion-xl-base-1.0' : model;
    const url = `${baseUri}/GenerateImages?query=${query}&model=${model}`;
    return this.client.requestBlob(url);
  }

  MultiUploadImage(payload: FormData): Observable<ApiResponse<boolean>> | null {
    const url = `${baseUri}/MultiUpload`;
    const res = this.client.post<boolean>(url, payload);
    return res;
  }

  UploadImage(payload: FormData): Observable<ApiResponse<ImageModel>> | null {
    const url = `${baseUri}/Upload`;

    const res = this.client.post<ImageModel>(url, payload);
    return res;
  }

  Search(query: string): Observable<ApiResponse<string>> | null {
    if (!query) return null;
    const url = `${baseUri}/Search`;
    const body = {
      query: query,
    };

    const res = this.client.get<string>(url, body);
    return res;
  }
}
