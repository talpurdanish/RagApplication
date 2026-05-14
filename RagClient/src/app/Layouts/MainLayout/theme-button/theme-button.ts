import { Component, OnInit } from '@angular/core';
import { ThemeService } from '../../../BussinessLogic/Helpers/Theme/theme';
import { lucideMoon, lucideSun } from '@ng-icons/lucide';
import { NgIcon, provideIcons } from '@ng-icons/core';
@Component({
  selector: 'app-theme',
  imports: [NgIcon],
   viewProviders: [provideIcons({ lucideMoon, lucideSun })],
  templateUrl: './theme-button.html',
  styleUrl: './theme-button.css',
})
export class ThemeButtonComponent implements OnInit {
  isDark: boolean;
  constructor(private themeService: ThemeService) {
    this.isDark = this.themeService.getTheme() == 'dark';
  }

  ngOnInit() {}

  setDark() {
    this.themeService.setTheme('dark');
    this.isDark = true;
  }

  setLight() {
    this.themeService.setTheme('light');
    this.isDark = false;
  }
}
