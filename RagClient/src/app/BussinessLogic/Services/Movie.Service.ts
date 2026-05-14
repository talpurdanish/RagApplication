// api/UserService.ts
import { Injectable } from '@angular/core';

import { ApiResponse } from '../Models/Generics/ApiResponse';
import { HttpApiClient } from './Generics/HttpApiClient';
import { Observable } from 'rxjs';
import { convertWeights, MovieModel, Weights } from '../Models/Movie.Model';
import createDefaultFilter, { convertFilter, Filter } from '../Models/Generics/Filter';

const baseUri = '/movies';
@Injectable({ providedIn: 'root' })
export class MovieService {
  constructor(private client: HttpApiClient) {}

  Get(id: number): Observable<ApiResponse<MovieModel>> {
    const url = `${baseUri}/GetMovie/${id}`;
    const res = this.client.get<MovieModel>(url, null);
    return res;
  }

  StartAnalysis(type: number = 1): Observable<ApiResponse<boolean>> {
    const url = `${baseUri}/StartAnalysis/${type}`;
    const res = this.client.get<boolean>(url, null);
    return res;
  }

  GetAll(filter?: Filter): Observable<ApiResponse<MovieModel>> | null {
    if (!filter) filter = createDefaultFilter();

    const url = `${baseUri}/GetMovies`;
    const res = this.client.get<MovieModel>(url, convertFilter(filter));
    return res;
  }

  UploadMovie(payload: FormData): Observable<ApiResponse<MovieModel>> | null {
    const url = `${baseUri}/Upload`;

    const res = this.client.post<MovieModel>(url, payload);
    return res;
  }

  Search(
    query?: string,
    weights?: Weights,
    type: number = 1,
  ): Observable<ApiResponse<string>> | null {
    if (!query || !weights) return null;
    weights.query = query;
    weights.type = type;
    const url = `${baseUri}/Search`;

    const res = this.client.get<string>(url, convertWeights(weights));
    return res;
  }
}
