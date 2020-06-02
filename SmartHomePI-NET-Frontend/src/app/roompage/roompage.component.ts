import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { CrudService } from '../_services/CRUD.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-roompage',
  templateUrl: './roompage.component.html',
  styleUrls: ['./roompage.component.css']
})
export class RoompageComponent implements OnInit {

  constructor(public activatedRoute: ActivatedRoute, public crudService: CrudService, public authService: AuthService) { }

  public roomIndex: string;

  rooms: Observable<any>;
  public selectedRoom: any;

  ngOnInit(): void {
    this.roomIndex = this.activatedRoute.snapshot.paramMap.get('id');

    console.log(this.roomIndex);

    this.crudService.getForUserId(parseInt(this.authService.decodedToken.nameid), 'room')
    .subscribe((rooms) => {
      // tslint:disable-next-line: no-string-literal
      this.rooms = rooms['roomList'];
      console.log(this.rooms);
      this.selectedRoom = this.rooms[this.roomIndex];
    });
  }

}
