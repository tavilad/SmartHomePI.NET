import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class CrudService {
  baseUrl = 'http://127.0.0.1:5000/api/';

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

}
