import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

export interface CreateBooking {
  customerName: string;
  customerEmail: string | null;
  customerPhone: string;
  bookingDate: string;
  numberOfGuests: number | null;
  specialRequests: string | null;
  address: string;
}

export interface BookingResponse {
  id: number;
  status: string;
  bookingDate: string;
  address: string;
}

@Injectable({ providedIn: 'root' })
export class BookingService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5077/api';

  create(booking: CreateBooking) {
    return this.http.post<BookingResponse>(`${this.baseUrl}/bookings`, booking);
  }
}