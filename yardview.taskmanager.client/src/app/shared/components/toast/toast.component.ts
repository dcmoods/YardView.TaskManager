import { AsyncPipe, NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


export type ToastType = 'success' | 'error' | 'info';

export interface ToastMessage {
  message: string;
  type: ToastType;
}


@Component({
  selector: 'app-toast',
  standalone: true,
  templateUrl: './toast.component.html',
  imports: [AsyncPipe, NgClass]
})
export class ToastComponent {

  private readonly toastSubject = new BehaviorSubject<ToastMessage | null>(null);
  readonly toast$ = this.toastSubject.asObservable();

  private timeoutId: ReturnType<typeof setTimeout> | null = null;

  show(message: string, type: ToastType = 'info'): void {
    this.toastSubject.next({ message, type });

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }

    this.timeoutId = setTimeout(() => {
      this.clear();
    }, 3000);
  }

  clear(): void {
    this.toastSubject.next(null);

    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }
  }

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  info(message: string): void {
    this.show(message, 'info');
  }

}
