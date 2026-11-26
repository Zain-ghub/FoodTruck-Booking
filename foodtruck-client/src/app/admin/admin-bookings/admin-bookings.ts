import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { AdminService, Booking } from '../admin.spec';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-admin-bookings',
  imports: [DatePipe],
  templateUrl: './admin-bookings.html',
  styleUrl: './admin-bookings.css',
})
export class AdminBookings implements OnInit {
  private admin = inject(AdminService);
  private auth = inject(AuthService);
  private router = inject(Router);

  bookings = signal<Booking[]>([]);
  loading = signal(true);
  error = signal('');

  ngOnInit() {
    this.load();
  }

  load() {
    this.admin.getBookings().subscribe({
      next: (list) => {
        this.bookings.set(list);
        this.loading.set(false);
      },
      error: (err) => this.handleError(err),
    });
  }

  setStatus(id: number, action: 'confirm' | 'decline') {
    const call = action === 'confirm' ? this.admin.confirm(id) : this.admin.decline(id);
    call.subscribe({
      next: () => this.load(),
      error: (err) => this.handleError(err),
    });
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  private handleError(err: { status?: number }) {
    this.loading.set(false);
    if (err.status === 401) {
      this.logout();
      return;
    }
    this.error.set('Something went wrong. Try again.');
  }
}