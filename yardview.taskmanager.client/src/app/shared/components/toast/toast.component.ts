import { AsyncPipe, NgClass } from '@angular/common';
import { Component, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ToastService } from '../../../core/services/toast.service';


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
  readonly toastService = inject(ToastService);
  readonly toast$ = this.toastService.toast$;
}
