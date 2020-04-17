import { Component, OnInit, HostBinding } from '@angular/core';
import { fadeInAnimation } from '../_animations';

export interface Tile {
  cols: number;
  rows: number;
  text: string;
  border: string;
 }

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})

export class DashboardComponent implements OnInit {

  tiles: Tile[] = [
    {text: 'Room 1', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 2', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 3', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 4', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 5', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 6', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 7', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 8', cols: 1, rows: 1 , border: '3px purple'},
    {text: 'Room 9', cols: 1, rows: 1 , border: '3px purple'},
    ];

  constructor() { }

  ngOnInit() {
  }

}
