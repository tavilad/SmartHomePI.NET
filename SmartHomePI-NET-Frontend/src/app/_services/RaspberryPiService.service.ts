import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RaspberryPiServiceService {
  public baseUrl: string = 'http://raspberrypi:8080/api/';

  constructor(private http: HttpClient) { }

  getReport(user: string, room: string) {
    return this.http.get(this.baseUrl + 'temperature/Report/' + user + '/' + room);
  }

  turnOnLight() {
    return this.http.get(this.baseUrl + 'light/lighton');
  }

  turnOffLight() {
    return this.http.get(this.baseUrl + 'light/lightoff');
  }

}
