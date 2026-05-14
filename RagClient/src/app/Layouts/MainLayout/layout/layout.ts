import { Component, OnInit } from '@angular/core';
import { Header } from '../header/header';
import { Footer } from '../footer/footer';
import { Sidebar } from '../sidebar/sidebar';
import { RouterOutlet } from '@angular/router';
import { ThemeService } from '../../../BussinessLogic/Helpers/Theme/theme';

import { Constants } from '../../../BussinessLogic/Helpers/Constants';
import { StorageService } from '../../../BussinessLogic/Services/Storage.Service';

@Component({
  selector: 'app-layout',
  imports: [Header, Footer, Sidebar, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit {
  collapsed: boolean;
  constructor(
    private themeService: ThemeService,
    private storageService: StorageService,
  ) {
    this.collapsed =
      this.storageService.get<boolean>(Constants.MENU_COLLAPSE_SETTINGS_KEY) ?? false;
  }

  collapseMenu(c: boolean) {
    this.collapsed = c;
    this.storageService.set<boolean>(Constants.MENU_COLLAPSE_SETTINGS_KEY, c);
  }

  ngOnInit(): void {
    this.collapsed =
      this.storageService.get<boolean>(Constants.MENU_COLLAPSE_SETTINGS_KEY) ?? false;
    this.themeService.initTheme();
  }
}
