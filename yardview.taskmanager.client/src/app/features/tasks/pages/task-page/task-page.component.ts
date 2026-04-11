import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CreateTaskRequest, TaskApiService, TaskFilter } from '../../../../core/services/task-api.service';
import { Task, TaskStatus } from '../../../../core/models/task.model';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { BrowserModule } from '@angular/platform-browser';
import { TaskListComponent } from '../../components/task-list/task-list.component';
import { AsyncPipe } from '@angular/common';
import { TaskFormComponent } from '../../components/task-form/task-form.component';

@Component({
  selector: 'app-task-page',
  standalone: true,
  templateUrl: './task-page.component.html',
  styleUrl: './task-page.component.css',
  imports: [ TaskListComponent, ConfirmDialogComponent, AsyncPipe, TaskFormComponent ],
})
export class TaskPageComponent implements OnInit {
  
  private readonly taskService = inject(TaskApiService);

  @ViewChild(TaskFormComponent) taskFormComponent?: TaskFormComponent;
  
  tasks$ = this.taskService.tasks$;
  loading$ = this.taskService.loading$;
  error$ = this.taskService.error$;
  
  isCreating = false;
  createError: string | null = null;
  
  selectedStatus: TaskStatus | 'all' = 'all';
  taskPendingDelete: Task | null = null;
  
  ngOnInit(): void {
    this.load();
  }

  load() {
    this.taskService.loadTasks(this.selectedStatus);
  }

  onFilterChange(status: TaskFilter) {
    this.selectedStatus = status;
    this.load();
  }

  onCreateTask(request: CreateTaskRequest): void {
    this.isCreating = true;
    this.createError = null;
    
    this.taskService.createTask(request).subscribe({
      next: () => {
        this.isCreating = false;
        this.taskFormComponent?.resetForm();
      },
      error: () => {
        this.isCreating = false;
        this.createError = 'Failed to create task.';
      },
    });
  }

  onStatusChange(event: { task: Task; status: TaskStatus }): void {
    this.taskService
      .updateTask(event.task.id, {
        title: event.task.title,
        description: event.task.description,
        status: event.status,
      })
      .subscribe();
  }

  openDeleteDialog(task: Task): void {
    console.log('Opening delete dialog for task:', task);
    this.taskPendingDelete = task;
  }

  closeDeleteDialog(): void {
    this.taskPendingDelete = null;
  }

  confirmDelete(): void {
    if (!this.taskPendingDelete) {
      return;
    }

    this.taskService.deleteTask(this.taskPendingDelete.id).subscribe({
      next: () => {
        this.closeDeleteDialog();
      },
    });
  }
}
