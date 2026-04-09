import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal, ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrls: ['./app.css'], 
})
export class App implements OnInit {
  public forecasts: WeatherForecast[] | null = null;

  constructor(private http: HttpClient, private cd: ChangeDetectorRef) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
      (result) => {
        console.log(result)
        this.forecasts = result;
        this.cd.detectChanges();
      },
      (error) => {
        console.error(error);
      }
    );
  }

  protected readonly title = signal('yardview.taskmanager.client');
}


interface WeatherForecast {
  date: Date;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
