// api/UserService.ts
import { Inject, Injectable } from '@angular/core';

import { ApiResponse } from '../Models/Generics/ApiResponse';
import { HttpApiClient } from './Generics/HttpApiClient';
import { Observable } from 'rxjs';
import { DocumentModel } from '../Models/Document.Model';

const baseUri = '/documents';
@Injectable({ providedIn: 'root' })
export class DocumentService {
  constructor(private client: HttpApiClient) {}

  InitDocument(payload: FormData): Observable<ApiResponse<boolean>> {
    const url = `${baseUri}/BulkImport`;
    const res = this.client.post<boolean>(url, payload);
    return res;
  }

  GetAll(): Observable<ApiResponse<DocumentModel>> | null {
    const url = `${baseUri}/GetAll`;
    const res = this.client.get<DocumentModel>(url, null);
    return res;
  }
  StartAnalysis(): Observable<ApiResponse<boolean>> {
    const url = `${baseUri}/StartAnalysis`;
    const res = this.client.get<boolean>(url, null);
    return res;
  }

  SearchDocument(
    searchString: string,
    previousMessages: string[],
  ): Observable<ApiResponse<string>> | null {
    if (!searchString) return null;
    const url = `${baseUri}/SearchDocument`;
    const body = {
      previousMessages,
      query: searchString,
    };

    const res = this.client.post<string>(url, body);
    return res;
  }
}
