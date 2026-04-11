import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  standalone: true,
})
export class LoadingSpinnerComponent {
  @Input() label = 'Loading...';
  @Input() size: 'sm' | 'md' = 'md';

  get spinnerClasses(): string {
    return this.size === 'sm'
      ? 'h-4 w-4 border-2'
      : 'h-8 w-8 border-[3px]';
  }
}
