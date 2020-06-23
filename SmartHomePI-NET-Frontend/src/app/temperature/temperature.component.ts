import { Component, OnInit, HostBinding, ViewChild, Input } from '@angular/core';
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
  baseUrl = 'http://raspberrypi:8080/api/temperature/';

  reading: any = {};

  @Input()
  roomId: any;

  isChartPaused: boolean;

  datasets: any[] = [{ label: 'Temperature', data: [] }, { label: 'Humidity', data: [] }];
  options: any = {
    scales: {
      xAxes: [{ type: 'realtime' }]
    },
    plugins: {
      streaming: {
        delay: 2000,
        duration: 50000,
        frameRate: 30,
        refresh: 2000,
        onRefresh: function (chart: any) {
          this.http.get(this.baseUrl + this.roomId).subscribe((reading) => {
            chart.data.datasets[0].data.push({ x: Date.now(), y: reading.temperature });
            chart.data.datasets[1].data.push({ x: Date.now(), y: reading.humidity });
          }
          );
        }.bind(this),
      },
    },
    tooltips: { mode: 'nearest', intersect: false }, hover: { mode: 'nearest', intersect: false },
  };

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  togglePause() {
    this.isChartPaused = !this.isChartPaused;
    this.options.scales.xAxes[0].pause = this.isChartPaused;
  }

}
