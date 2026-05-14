import { Injectable } from '@angular/core';
import { StorageService } from '../../Services/Storage.Service';
import { Constants } from '../Constants';

export type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  constructor(private storageService: StorageService) {}

  setTheme(theme: Theme) {
    document.documentElement.classList.remove('light', 'dark');
    document.documentElement.classList.add(theme);
    this.storageService.set(Constants.THEME_STORAGE_KEY, theme);
  }

  getTheme(): Theme {
    const saved = this.storageService.get(Constants.THEME_STORAGE_KEY) as Theme | null;
    return saved ?? 'light';
  }

  initTheme() {
    const theme = this.getTheme();
    this.setTheme(theme);
  }
}
