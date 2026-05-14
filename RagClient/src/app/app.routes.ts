import { Routes } from '@angular/router';
import { Layout } from './Layouts/MainLayout/layout/layout';
import { HomeComponent } from './Components/home/home';
import { RouterPaths } from './RouterPaths';
import { DocumentComponent } from './Components/document/document';
import { ImagesComponent } from './Components/images/images';
import { GenerateImagesComponent } from './Components/generate.image/generate.images';
import { MoviesComponent } from './Components/movies/movies';
import { ConfigMoviesComponent } from './Components/config.movies/movies.config';
import { DocumentChatComponent } from './Components/documentchat/document.chat';

export const routes: Routes = [
  {
    path: '',
    component: Layout,
    children: [
      {
        path: RouterPaths.dashboard,
        component: HomeComponent,
      },
      {
        path: RouterPaths.documents,
        component: DocumentComponent,
      },
      {
        path: RouterPaths.documentsChat,
        component: DocumentChatComponent,
      },
      {
        path: RouterPaths.images,
        component: ImagesComponent,
      },
      {
        path: RouterPaths.generateImages,
        component: GenerateImagesComponent,
      },
      {
        path: RouterPaths.movies,
        component: MoviesComponent,
      },
      {
        path: RouterPaths.configmovies,
        component: ConfigMoviesComponent,
      },
      {
        path: '',
        redirectTo: RouterPaths.dashboard,
        pathMatch: 'full',
      },
    ],
  },
  { path: '**', redirectTo: RouterPaths.dashboard },
];
