import { Routes } from '@angular/router';
import { Menu } from './menu/menu';

export const routes: Routes = [
  { path: 'menu', component: Menu },
  { path: '', redirectTo: 'menu', pathMatch: 'full' }
];