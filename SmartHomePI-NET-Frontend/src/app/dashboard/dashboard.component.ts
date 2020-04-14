import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations/index';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  animations: [fadeInAnimation]
})

export class DashboardComponent implements OnInit {

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  constructor() { }

  ngOnInit() {
  }

}
