import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';
import { RoomService } from '../_services/room.service';
import { AuthService } from '../_services/auth.service';

export interface Tile {
  cols: number;
  rows: number;
  text: string;
  border: string;
 }

export interface DialogData {
  roomName: string;
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

  roomName: string;
  model: any = {};

  constructor(public dialog: MatDialog, private alertify: AlertifyService,
              private roomService: RoomService, private authService: AuthService) { }

  ngOnInit() {
  }

  openDialog() {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px', data : {roomName : this.roomName}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        console.log(result);
        this.model.userId = parseInt(this.authService.decodedToken.nameid);
        this.model.roomName = result;
        console.log(this.model);
        this.roomService.createRoom(this.model).subscribe(() => {
          this.alertify.success('Created room ' + this.model.roomName);
        }, error => {
          this.alertify.error(error);
        });
      }
    });
  }

}

@Component({
  selector: 'app-dialog',
  templateUrl: 'dialog.html',
})

export class DialogComponent {

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

}
