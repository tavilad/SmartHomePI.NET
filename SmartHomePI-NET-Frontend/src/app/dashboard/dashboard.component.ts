import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';
import { CrudService } from '../_services/CRUD.service';
import { AuthService } from '../_services/auth.service';
import { Observable } from 'rxjs';

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

  tiles: Tile[] = [];

  roomName: string;
  model: any = {};
  rooms: Observable<any>;

  constructor(public dialog: MatDialog, private alertify: AlertifyService,
              private crudService: CrudService, private authService: AuthService) {
               }

  ngOnInit() {
    this.initRooms();
  }

  openDialog() {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px', data : {roomName : this.roomName}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        console.log(result);
        // tslint:disable-next-line: radix
        this.model.userId = parseInt(this.authService.decodedToken.nameid);
        this.model.roomName = result;
        console.log(this.model);
        this.crudService.create(this.model, 'room').subscribe(() => {
          this.alertify.success('Created room ' + this.model.roomName);
          this.tiles.push({text: this.model.roomName, cols: 1, rows: 1 , border: ''});
        }, error => {
          this.alertify.error(error);
        });
      }
    });
  }

  initRooms() {
    // tslint:disable-next-line: radix
    this.crudService.getForUserId(parseInt(this.authService.decodedToken.nameid), 'room')
    .subscribe((rooms) => {
      // tslint:disable-next-line: no-string-literal
      this.rooms = rooms['roomList'];
      console.log(this.rooms);
      this.rooms.forEach((room) => {
        this.tiles.push({text: room.roomName, cols: 1, rows: 1 , border: ''});
      });
    });
  }

  removeRoom(selectedRoomId: string) {
    console.log('remove room');
    this.alertify.success(selectedRoomId);
    this.crudService.deleteByName(selectedRoomId, this.authService.decodedToken.nameid, 'room').subscribe(() => {
      const index = this.tiles.findIndex(tile => tile.text === selectedRoomId);
      if (index !== -1) {
        this.tiles.splice(index, 1);
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
