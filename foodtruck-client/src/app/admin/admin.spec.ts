import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

export interface Booking {
  id: number;
  customerName: string;
  customerEmail: string | null;
  customerPhone: string;
  bookingDate: string;
  numberOfGuests: number | null;
  specialRequests: string | null;
  address: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5077/api';

  getBookings() {
    return this.http.get<Booking[]>(`${this.baseUrl}/bookings`);
  }
  confirm(id: number) {
    return this.http.post<void>(`${this.baseUrl}/bookings/${id}/confirm`, {});
  }
  decline(id: number) {
    return this.http.post<void>(`${this.baseUrl}/bookings/${id}/decline`, {});
  }
}