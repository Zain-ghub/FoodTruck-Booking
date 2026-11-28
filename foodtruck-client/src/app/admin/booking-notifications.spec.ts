import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { AuthService } from '../auth.service';

export interface BookingCreatedEvent {
  id: number;
  customerName: string;
  bookingDate: string;
  address: string;
}

@Injectable({ providedIn: 'root' })
export class BookingNotificationsService {
  private auth = inject(AuthService);
  private connection: signalR.HubConnection | null = null;
  private subject = new Subject<BookingCreatedEvent>();

  // Components subscribe to this stream
  readonly bookingCreated$: Observable<BookingCreatedEvent> = this.subject.asObservable();

  async start(): Promise<void> {
    if (this.connection) return;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5077/hubs/bookings', {
        // Sent as ?access_token=... which your OnMessageReceived handler reads
        accessTokenFactory: () => this.auth.token ?? '',
      })
      .withAutomaticReconnect()
      .build();

    // Must match the server's event name exactly, including the lowercase "b"
    this.connection.on('bookingCreated', (payload: BookingCreatedEvent) => {
      this.subject.next(payload);
    });

    try {
      await this.connection.start();
    } catch (err) {
      console.error('SignalR connection failed', err);
      this.connection = null;
    }
  }

  async stop(): Promise<void> {
    await this.connection?.stop();
    this.connection = null;
  }
}