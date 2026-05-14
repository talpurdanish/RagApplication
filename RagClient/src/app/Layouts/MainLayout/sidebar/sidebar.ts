import { Component, Input, OnInit, signal } from '@angular/core';
import { MenuItem } from './MenuItem';
import {
  lucideHouse,
  lucideUsers,
  lucideZoomIn,
  lucideChevronRight,
  lucideMailbox,
  lucideRabbit,
  lucideIdCardLanyard,
  lucideFile,
  lucideImage,
  lucidePaintbrush2,
  lucideFilm,
  lucideSettings,
  lucideMessageSquare,
  lucideImages,
  lucideVideotape,
  lucideFileSearch2,
} from '@ng-icons/lucide';
import { RouterPaths } from '../../../RouterPaths';
import { Constants, Role, Roles } from '../../../BussinessLogic/Helpers/Constants';
import { Router } from '@angular/router';
import { StorageService } from '../../../BussinessLogic/Services/Storage.Service';
import { NgIcon, provideIcons } from '@ng-icons/core';

@Component({
  selector: 'app-sidebar',
  imports: [NgIcon],
  providers: [NgIcon],
  viewProviders: [
    provideIcons({
      lucideHouse,
      lucideUsers,
      lucideZoomIn,
      lucideChevronRight,
      lucideMailbox,
      lucideRabbit,
      lucideIdCardLanyard,
      lucideFile,
      lucideImage,
      lucidePaintbrush2,
      lucideFilm,
      lucideMessageSquare,
      lucideImages,
      lucideVideotape,
      lucideFileSearch2,
    }),
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar implements OnInit {
  @Input() collapsed: boolean = false;

  openIndices = signal<number[]>([]);
  activeItem = signal<string>(RouterPaths.dashboard);
  iconSize = signal<string>('16');
  constructor(private router: Router) {}

  ngOnInit(): void {
    var i = this.collapsed ? '24' : '16';
    this.iconSize.set(i);
  }
  toggleAccordion = (index: number) => {
    this.openIndices.update((arr) => {
      if (arr.filter((a) => a == index).length > 0) {
        return arr.filter((a) => a != index);
      } else {
        return [...arr, index];
      }
    });
  };

  openIndicesHas(index: number): boolean {
    return this.openIndices().find((o) => o == index) != undefined;
  }

  menuItems: MenuItem[] = [
    {
      icon: lucideHouse,
      title: 'Dashboard',
      link: RouterPaths.dashboard,
      tooltip: 'Dashboard',
    },
    {
      icon: lucideImages,
      title: 'RAG Images',
      items: [
        {
          icon: lucideImage,
          title: 'Images',
          link: RouterPaths.images,
          tooltip: 'Images',
        },
        {
          icon: lucidePaintbrush2,
          title: 'Generate Images',
          link: RouterPaths.generateImages,
          tooltip: 'Generate Images',
        },
      ],
    },
    {
      icon: lucideVideotape,
      title: 'RAG Movies',
      items: [
        {
          icon: lucideFilm,
          title: 'Movies',
          link: RouterPaths.movies,
          tooltip: 'Movies',
        },
        {
          icon: lucideSettings,
          title: 'Config Movies',
          link: RouterPaths.configmovies,
          tooltip: 'Config Movies',
        },
      ],
    },
    {
      icon: lucideFileSearch2,
      title: 'RAG Documents',
      items: [
        {
          icon: lucideFile,
          title: 'Documents',
          link: RouterPaths.documents,
          tooltip: 'Documents',
        },
        {
          icon: lucideMessageSquare,
          title: 'Documents Chat',
          link: RouterPaths.documentsChat,
          tooltip: 'Documents Chat',
        },
      ],
    },
  ];

  findActiveSubmenuIndex(activeLink: string) {
    for (let i = 0; i < this.menuItems.length; i++) {
      const item = this.menuItems[i];
      if (item.items && Array.isArray(item.items)) {
        const found = item.items.some((subItem) => subItem.link == activeLink);
        if (found) {
          return i;
        }
      }
    }
    return -1;
  }

  updateActiveItem = (pathname: string) => {
    let path = pathname.endsWith('/') ? pathname.slice(0, -1) : pathname;

    const segments = path.split('/');
    const lastSegment = segments[segments.length - 1];
    if (/^\d+$/.test(lastSegment)) {
      segments.pop();
    }

    const basePath = segments.join('/');
    const inSubMenuIndex = this.findActiveSubmenuIndex(basePath);
    if (inSubMenuIndex > -1 && !this.openIndices().find((a) => a == inSubMenuIndex)) {
      this.toggleAccordion(inSubMenuIndex);
    }
    this.activeItem.set(basePath);
  };

  navigateToHref(link: any) {
    this.activeItem.set(link);
    this.router.navigateByUrl(link);
  }

  findParentIndex(activeLink: string) {
    for (let i = 0; i < this.menuItems.length; i++) {
      const item = this.menuItems[i];
      if (item.items && item.items.some((subItem) => subItem.link === activeLink)) {
        return i;
      }
    }
    return -1;
  }
}
