import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations/index';
import { HttpClient } from '@angular/common/http';
import { Observable, interval } from 'rxjs';
import { flatMap } from 'rxjs/operators';
import 'chartjs-plugin-streaming';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.css'],
})

export class TemperatureComponent implements OnInit {
  baseUrl = 'http://localhost:5000/api/temperature';

  reading: any = {};

  datasets: any[] = [{ data: [] }, { data: [] }];
  options: any = {
    scales: {
      xAxes: [{ type: 'realtime' }]
    },
    plugins: {
      streaming: {
        delay: 2000,
        onRefresh: function (chart: any) {
          chart.data.datasets.forEach(function (dataset: any) {
            dataset.data.push({
              x: Date.now(),
              y: Math.random()
            });
          });
        },
      },
    }
  };

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getTemperatureAndHumidity();
  }

  getTemperatureAndHumidity() {
    interval(2000).pipe(flatMap(() => this.http.get(this.baseUrl))).subscribe((reading) => this.reading = reading);
  }

}
