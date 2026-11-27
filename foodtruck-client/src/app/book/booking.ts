import { Component, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { BookingResponse, BookingService } from './booking.spec';

function futureDate(control: AbstractControl): ValidationErrors | null {
  if (!control.value) return null;
  return new Date(control.value).getTime() > Date.now() ? null : { pastDate: true };
}

@Component({
  selector: 'app-book',
  imports: [ReactiveFormsModule],
  templateUrl: './book.html',
  styleUrl: './book.css',
})
export class Book {
  private fb = inject(FormBuilder);
  private bookings = inject(BookingService);

  sending = signal(false);
  confirmation = signal<BookingResponse | null>(null);
  serverErrors = signal<string[]>([]);

  form = this.fb.nonNullable.group({
    customerName: ['', [Validators.required, Validators.maxLength(100)]],
    customerPhone: ['', [Validators.required, Validators.pattern(/^\+?[0-9\s\-()]{7,20}$/)]],
    customerEmail: ['', [Validators.email]],
    bookingDate: ['', [Validators.required, futureDate]],
    numberOfGuests: [null as number | null, [Validators.min(1), Validators.max(700)]],
    address: ['', [Validators.required]],
    specialRequests: [''],
  });

  get f() {
    return this.form.controls;
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.sending.set(true);
    this.serverErrors.set([]);
    const v = this.form.getRawValue();

    this.bookings
      .create({
        customerName: v.customerName.trim(),
        customerPhone: v.customerPhone.trim(),
        customerEmail: v.customerEmail.trim() || null,
        bookingDate: v.bookingDate,
        numberOfGuests: v.numberOfGuests,
        specialRequests: v.specialRequests.trim() || null,
        address: v.address.trim(),
      })
      .subscribe({
        next: (res) => {
          this.confirmation.set(res);
          this.sending.set(false);
        },
        error: (err: HttpErrorResponse) => {
          this.sending.set(false);
          const apiErrors = err.error?.errors;
          this.serverErrors.set(
            apiErrors
              ? (Object.values(apiErrors).flat() as string[])
              : ['Something went wrong. Please try again.']
          );
        },
      });
  }

  reset() {
    this.form.reset();
    this.confirmation.set(null);
    this.serverErrors.set([]);
  }
}