import { Component, inject } from '@angular/core';
import { AsyncPipe, DecimalPipe } from '@angular/common';
import { map } from 'rxjs';
import { MenuService } from './menu.spec';
import { MenuItem } from './menu-item/menu-item';

@Component({
  selector: 'app-menu',
  imports: [AsyncPipe, DecimalPipe],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})
export class Menu {
  private menuService = inject(MenuService);

  groups$ = this.menuService.getMenu().pipe(
    map((list) => {
      const groups: Record<string, MenuItem[]> = {};
      for (const item of list) {
        (groups[item.category] ??= []).push(item);
      }
      return Object.entries(groups).map(([category, items]) => ({ category, items }));
    })
  );
}