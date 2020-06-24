import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RaspberryPiServiceService {
  public baseUrl: string = 'http://raspberrypi:8080';
  public cameraUrl: string = this.baseUrl + '/stream.mjpeg/';

  constructor(private http: HttpClient) { }

  getReport(user: string, room: string) {
    return this.http.get(this.baseUrl + '/api/temperature/Report/' + user + '/' + room);
  }

  turnOnLight() {
    return this.http.get(this.baseUrl + '/api/light/lighton');
  }

  turnOffLight() {
    return this.http.get(this.baseUrl + '/api/light/lightoff');
  }

  getDistance() {
    return this.http.get(this.baseUrl + '/api/distance/get');
  }

}
