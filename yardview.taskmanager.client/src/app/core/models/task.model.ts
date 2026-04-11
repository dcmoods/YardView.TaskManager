export interface Task {
  id: number;
  title: string;
  description?: string;
  status: 'todo' | 'in_progress' | 'done';
  createdAt: string;
}

export type TaskStatus = 'todo' | 'in_progress' | 'done';