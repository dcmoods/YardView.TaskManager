import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskPageComponent } from './features/tasks/pages/task-page/task-page.component';
import { ShellComponent } from './shared/layout/shell/shell.component';

const routes: Routes = [
  //{ path: '', component: TaskPageComponent }
  {
    path: '',
    component: ShellComponent,
    children: [
      {
        path: '',
        component: TaskPageComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
