import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  baseUrl = 'http://127.0.0.1:5000/api/room/';

  constructor(private http: HttpClient, private alertify: AlertifyService) { }

  createRoom(model: any) {
    return this.http.post(this.baseUrl + 'create', model);
  }

  deleteRoom(model: any) {
    return this.http.delete(this.baseUrl + 'delete', model);
  }

  updateRoom(model: any) {
    return this.http.put(this.baseUrl + 'update', model);
  }

  getAllRooms() {

  }

}
