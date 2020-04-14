import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations/index';

@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.css'],
  animations: [fadeInAnimation]
})

export class TemperatureComponent implements OnInit {

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  constructor() { }

  ngOnInit() {
  }

}
