import { Routes } from '@angular/router';
import { Menu } from './menu/menu';
import { AdminBookings } from './admin/admin-bookings/admin-bookings';
import { authGuard } from './auth.guard';
import { Login } from './login/login/login';
import { Book } from './book/booking';

export const routes: Routes = [
  { path: 'menu', component: Menu },
  { path: 'book', component: Book },
  { path: '', redirectTo: 'menu', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'admin', component: AdminBookings, canActivate: [authGuard] }
];