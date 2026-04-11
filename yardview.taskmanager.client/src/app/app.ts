import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrls: ['./app.css'], 
})
export class App implements OnInit {

  constructor() {}

  ngOnInit() {
  }

 

  protected readonly title = signal('yardview.taskmanager.client');
}

