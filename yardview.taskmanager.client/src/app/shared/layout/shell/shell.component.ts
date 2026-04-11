import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ContainerComponent } from '../container/container.component';
import { ToastComponent } from '../../components/toast/toast.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  templateUrl: './shell.component.html',
  imports: [RouterOutlet, ContainerComponent, ToastComponent],
})
export class ShellComponent {}
