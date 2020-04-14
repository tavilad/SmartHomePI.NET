import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations/index';

@Component({
  selector: 'app-camera',
  templateUrl: './camera.component.html',
  styleUrls: ['./camera.component.css'],
  animations: [fadeInAnimation]
})

export class CameraComponent implements OnInit {

  @HostBinding('@fadeInAnimation') get fadeInAnimation() {
    return '';
  }

  constructor() { }

  ngOnInit() {
  }

}
