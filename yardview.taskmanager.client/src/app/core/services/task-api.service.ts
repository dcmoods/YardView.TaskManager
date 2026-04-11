import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import {
  BehaviorSubject,
  tap,
} from 'rxjs';
import { Task, TaskStatus } from '../models/task.model';
import { environment } from '../../../environments/environment';

export type TaskFilter = TaskStatus | 'all';

export interface CreateTaskRequest {
  title: string;
  description?: string;
  status: TaskStatus;
}

export interface UpdateTaskRequest {
  title: string;
  description?: string;
  status: TaskStatus;
}

@Injectable({
  providedIn: 'root',
})
export class TaskApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tasks`;

  private readonly tasksSubject = new BehaviorSubject<Task[]>([]);
  readonly tasks$ = this.tasksSubject.asObservable();

  private readonly loadingSubject = new BehaviorSubject<boolean>(false);
  readonly loading$ = this.loadingSubject.asObservable();

  private readonly errorSubject = new BehaviorSubject<string | null>(null);
  readonly error$ = this.errorSubject.asObservable();

  private currentStatusFilter: TaskFilter = 'all';

  getCurrentFilter(): TaskFilter {
    return this.currentStatusFilter;
  }

  loadTasks(status?: TaskStatus | 'all') {
    this.currentStatusFilter = status || 'all';
    this.loadingSubject.next(true);
    this.errorSubject.next(null);
    
    let params = new HttpParams();
    
    if (status && status !== 'all') {
      params = params.set('status', status);
    }
    
    this.http.get<Task[]>(this.baseUrl, { params })
      .subscribe({
        next: (tasks) => {
          this.tasksSubject.next(tasks);
          this.loadingSubject.next(false);
        },
        error: () => {
          this.errorSubject.next('Failed to load tasks');
          this.loadingSubject.next(false);
        },
    });
  }

  refreshTasks(): void {
    this.loadTasks(this.currentStatusFilter);
  }

  createTask(request: CreateTaskRequest) {
    return this.http.post<Task>(this.baseUrl, request).pipe(
      tap(() => this.refreshTasks())
    );
  }

  updateTask(id: number, request: UpdateTaskRequest) {
    return this.http.put<Task>(`${this.baseUrl}/${id}`, request).pipe(
      tap(() => this.refreshTasks())
    );
  }

  deleteTask(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`).pipe(
      tap(() => this.refreshTasks())
    );
  }


}