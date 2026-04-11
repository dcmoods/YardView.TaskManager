import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Task, TaskStatus } from '../../../../core/models/task.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-list',
  standalone: true,
  templateUrl: './task-list.component.html',
  imports: [CommonModule],
})
export class TaskListComponent {
  @Input({ required: true }) tasks: Task[] = [];

  @Output() statusChange = new EventEmitter<{ task: Task; status: TaskStatus }>();
  @Output() deleteTask = new EventEmitter<Task>();


  recentlyUpdated = new Set<number>();

  onStatusChanged(task: Task, status: string): void {
    this.statusChange.emit({
      task,
      status: status as TaskStatus,
    });

    this.recentlyUpdated.add(task.id);

    setTimeout(() => {
      this.recentlyUpdated.delete(task.id);
    }, 600);
  }

  trackById(_: number, task: Task) {
    return task.id;
  }

  getStatusLabel(status: Task['status']): string {
    switch (status) {
      case 'todo':
        return 'To Do';
      case 'in_progress':
        return 'In Progress';
      case 'done':
        return 'Done';
      default:
        return status;
    }
  }

  getStatusClasses(status: TaskStatus): string {
    switch (status) {
      case 'todo':
        return 'bg-blue-100 text-blue-700 border-blue-200 focus:ring-blue-200';
      case 'in_progress':
        return 'bg-amber-100 text-amber-700 border-amber-200 focus:ring-amber-200';
      case 'done':
        return 'bg-green-100 text-green-700 border-green-200 focus:ring-green-200';
    }
  }
}
