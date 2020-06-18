import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations/index';
import { HttpClient } from '@angular/common/http';
import { Observable, interval } from 'rxjs';
import { flatMap } from 'rxjs/operators';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.css'],
})

export class TemperatureComponent implements OnInit {
  baseUrl = 'http://localhost:5000/api/temperature';
  reading: any = {};
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getTemperatureAndHumidity();
  }

  getTemperatureAndHumidity() {
    interval(2000).pipe(flatMap(() => this.http.get(this.baseUrl))).subscribe((reading) => this.reading = reading);
  }

}
