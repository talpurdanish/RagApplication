import { Injectable } from '@angular/core';

export interface IStorageService {
  set<T>(key: string, value: T): void;
  get<T>(key: string): T | null;
  remove(key: string): void;
  clear(): void;
}
@Injectable({ providedIn: 'root' })
export class StorageService implements IStorageService {
  clear(): void {
    sessionStorage.clear();
  }
  set<T>(key: string, value: T) {
    sessionStorage.setItem(key, JSON.stringify(value));
  }
  get<T>(key: string): T | null {
    const item = sessionStorage.getItem(key);
    return item ? JSON.parse(item) : null;
  }
  remove(key: string) {
    sessionStorage.removeItem(key);
  }
}
