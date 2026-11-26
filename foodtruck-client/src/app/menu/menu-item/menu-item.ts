import { Component } from '@angular/core';

// @Component({
//   imports: [],
//   selector: 'app-menu-item',
//   styleUrl: './menu-item.css',
//   templateUrl: './menu-item.html',
// })
export interface MenuItem {
  id?: number;
  name: string;
  description: string | null;
  price: number;
  category: string;
  isAvailable: boolean;
}