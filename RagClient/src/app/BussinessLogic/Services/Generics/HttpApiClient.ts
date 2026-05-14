// api/HttpClientApiClient.ts
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { normalizePayload } from './ApiUtils';
import { StorageService, IStorageService } from '../Storage.Service';

import { Observable, of } from 'rxjs';
import { Constants } from '../../Helpers/Constants';

@Injectable()
export class HttpApiClient {
  baseUrl = Constants.API_URL_RAG;
  constructor(private http: HttpClient) {}

  get<T>(url: string, payload?: any): Observable<any> {
    const { params, queryString } = normalizePayload(payload);
    const finalUrl = queryString ? `${this.baseUrl}${url}?${queryString}` : `${this.baseUrl}${url}`;
    return this.http.get<T>(finalUrl, {
      params,
    });
  }

  post<T>(url: string, body: any = {}): Observable<any> {
    return this.http.post<T>(`${this.baseUrl}${url}`, body);
  }

  put<T>(url: string, body: any = {}): Observable<any> {
    return this.http.put<T>(`${this.baseUrl}${url}`, body);
  }

  delete<T>(url: string): Observable<any> {
    return this.http.delete<T>(`${this.baseUrl}${url}`);
  }

  requestBlob(url: string, query?: string): Observable<Blob> {
    const finalUrl = query ? `${this.baseUrl}${url}?query=${query}` : `${this.baseUrl}${url}`;
    return this.http.post(finalUrl, {}, { responseType: 'blob' });
  }
}
