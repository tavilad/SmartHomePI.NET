import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from './alertify.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CrudService {
  baseUrl = 'http://localhost:5000/api/';

  constructor(private http: HttpClient, private alertify: AlertifyService) { }

  create(model: any, tableName: string) {
    return this.http.post(this.baseUrl + tableName + '/create', model);
  }

  delete(model: any, tableName: string) {
    return this.http.delete(this.baseUrl + tableName  + '/delete', model);
  }

  update(model: any, tableName: string) {
    return this.http.put(this.baseUrl + tableName + '/update', model);
  }

  getAll() {

  }

  getForUserId(userId: any, tableName: string) {
    return this.http.get(this.baseUrl + tableName + '/forUser/' + userId);
  }

  getById(id: any, tableName: string) {
    return this.http.get(this.baseUrl + tableName + id);
  }

  deleteByName(roomName: string, userId: any, tableName: string) {
    return this.http.delete(this.baseUrl + tableName  + '/DeleteByName/' + roomName + '/' + userId);
  }

}
