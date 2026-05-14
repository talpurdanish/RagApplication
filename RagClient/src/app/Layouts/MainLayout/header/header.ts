import { Component, Output, Input, EventEmitter } from '@angular/core';

import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideMenu } from '@ng-icons/lucide';
import { ThemeButtonComponent } from '../theme-button/theme-button';

@Component({
  selector: 'app-header',
  imports: [NgIcon, ThemeButtonComponent],
  viewProviders: [provideIcons({ lucideMenu })],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  @Output() collapseMenuCall = new EventEmitter<boolean>();
  @Input() collapsed: boolean = false;

  collapseMenu() {
    this.collapsed = !this.collapsed;
    this.collapseMenuCall.emit(this.collapsed);
  }
}
