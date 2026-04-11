import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CreateTaskRequest, TaskApiService, TaskFilter } from '../../../../core/services/task-api.service';
import { Task, TaskStatus } from '../../../../core/models/task.model';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { TaskListComponent } from '../../components/task-list/task-list.component';
import { AsyncPipe } from '@angular/common';
import { TaskFormComponent } from '../../components/task-form/task-form.component';
import { ToastService } from '../../../../core/services/toast.service';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-task-page',
  standalone: true,
  templateUrl: './task-page.component.html',
  imports: [ TaskListComponent, ConfirmDialogComponent, AsyncPipe, TaskFormComponent, LoadingSpinnerComponent ],
})
export class TaskPageComponent implements OnInit {
  
  private readonly taskService = inject(TaskApiService);
  private readonly toastService = inject(ToastService);

  @ViewChild(TaskFormComponent) taskFormComponent?: TaskFormComponent;
  
  tasks$ = this.taskService.tasks$;
  loading$ = this.taskService.loading$;
  error$ = this.taskService.error$;
  
  showCreateForm = false;
  isCreating = false;
  createError: string | null = null;
  
  selectedStatus: TaskStatus | 'all' = 'all';
  
  isDeleting = false;
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

  openCreateForm() {
    this.showCreateForm = true;
  }

  closeCreateForm() {
    this.showCreateForm = false;
    this.createError = null;
  }

  onCreateTask(request: CreateTaskRequest): void {
    this.isCreating = true;
    this.createError = null;
    
    this.taskService.createTask(request).subscribe({
      next: () => {
        this.isCreating = false;
        this.showCreateForm = false;
        this.taskFormComponent?.resetForm();
        this.toastService.success('Task created successfully!');
      },
      error: () => {
        this.isCreating = false;
        this.createError = 'Failed to create task.';
        this.toastService.error('Failed to create task.');
      },
    });
  }

  onStatusChange(event: { task: Task; status: TaskStatus }): void {
    this.taskService
      .updateTask(event.task.id, {
        title: event.task.title,
        description: event.task.description,
        status: event.status,
        dueDate: event.task.dueDate ?? null,
      })
      .subscribe(
        {
          next: () => {
            this.toastService.success('Task updated successfully!');
          },
          error: () => {
            this.toastService.error('Failed to update task.');
          },
        }
      );
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

    this.isDeleting = true;
    this.taskService.deleteTask(this.taskPendingDelete.id).subscribe({
      next: () => {
        this.closeDeleteDialog();
        this.isDeleting = false;
        this.toastService.success('Task deleted successfully!');
      },
      error: () => {
        this.isDeleting = false;
        this.toastService.error('Failed to delete task.');
      },
    });
  }
}
