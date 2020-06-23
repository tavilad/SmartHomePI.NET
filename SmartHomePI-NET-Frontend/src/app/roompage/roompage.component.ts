import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { CrudService } from '../_services/CRUD.service';
import { AuthService } from '../_services/auth.service';
import { RaspberryPiServiceService } from '../_services/RaspberryPiService.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-roompage',
  templateUrl: './roompage.component.html',
  styleUrls: ['./roompage.component.css']
})
export class RoompageComponent implements OnInit {

  constructor(public activatedRoute: ActivatedRoute, public crudService: CrudService, public authService: AuthService,
              public raspberryService: RaspberryPiServiceService, public alertifyService: AlertifyService) { }

  public roomIndex: string;

  rooms: Observable<any>;
  public selectedRoom: any;

  lightOn: boolean;

  ngOnInit(): void {
    this.roomIndex = this.activatedRoute.snapshot.paramMap.get('id');

    this.crudService.getForUserId(parseInt(this.authService.decodedToken.nameid), 'room')
      .subscribe((rooms) => {
        // tslint:disable-next-line: no-string-literal
        this.rooms = rooms['roomList'];
        console.log(this.rooms);
        this.selectedRoom = this.rooms[this.roomIndex];
        this.raspberryService.baseUrl = this.selectedRoom.ipAddress;
        console.log(this.raspberryService.baseUrl);
      });
  }

  getReport() {
    this.raspberryService.getReport(this.authService.decodedToken.unique_name, this.selectedRoom.roomName)
      .subscribe(() => {console.log('sent report'); }, error => {
        this.alertifyService.error(error);
      });
  }

  controlLight(e) {
    if (!this.lightOn) {
      this.raspberryService.turnOnLight().subscribe(() => {console.log('on'); }, error => {
        this.alertifyService.error(error);
      });
      this.lightOn = true;
    } else {
      this.raspberryService.turnOffLight().subscribe(() => {console.log('on'); }, error => {
        this.alertifyService.error(error);
      });
      this.lightOn = false;
    }
  }
}


