// api/UserService.ts
import { Injectable } from '@angular/core';

import { ApiResponse } from '../Models/Generics/ApiResponse';
import { HttpApiClient } from './Generics/HttpApiClient';
import { Observable } from 'rxjs';

const baseUri = '/agents';

export interface TaskItem {
  id: number;
  name: string;
  description: string;
  isDone: boolean;
}

export function createTaskItem(raw: any): TaskItem {
  return {
    id: raw.id ?? -1,
    name: raw.name ?? '',
    description: raw.description ?? '',
    isDone: raw.isDone ?? false,
  };
}

export interface TaskResponse {
  success: boolean;
  message: string;
  data: TaskItem[];
}

export function createTask(raw: any): TaskResponse {
  console.log(raw);
  return {
    success: raw.success ?? false,
    message: raw.message ?? '',
    data: Array.isArray(raw.data) ? raw.data.map((item: any) => createTaskItem(item)) : [],
  };
}
@Injectable({ providedIn: 'root' })
export class AgentService {
  constructor(private client: HttpApiClient) {}

  Run(input: string, sessionId: string): Observable<ApiResponse<TaskResponse>> | null {
    if (input == '' || sessionId == '') return null;
    const url = `${baseUri}/run?input=${input}&sessionId=${sessionId}`;
    const res = this.client.get<TaskResponse>(url, null);
    return res;
  }
}
