import { Component, EventEmitter, Input, Output, output, ViewChild } from '@angular/core';
import { Task, TaskStatus } from '../../../../core/models/task.model';
import { NgIf } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { CreateTaskRequest } from '../../../../core/services/task-api.service';



@Component({
  selector: 'app-task-form',
  imports: [FormsModule, NgIf],
  templateUrl: './task-form.component.html',
  standalone: true,
})
export class TaskFormComponent {
  @Input() isSaving = false;
  @Output() createTask = new EventEmitter<CreateTaskRequest>();

  @ViewChild('taskForm') taskForm?: NgForm;

  title = '';
  description = '';
  status: TaskStatus = 'todo';

  submit(form: NgForm): void {
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    if (!this.title.trim()) {
      return;
    }

    this.createTask.emit({
      title: this.title.trim(),
      description: this.description.trim() || undefined,
      status: this.status,
    });
  }

  resetForm(): void {
    this.taskForm?.resetForm({
      title: '',
      description: '',
      status: 'todo',
    });
  }
}
