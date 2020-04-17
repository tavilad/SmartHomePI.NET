import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Output() cancelLogin = new EventEmitter();
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit(): void {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in successfuly');
      this.cancelLogin.emit(false);
    }, error => {
      this.alertify.error('Failed to login');
    }, () => {
      this.router.navigate(['/dashboard']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  cancel() {
    this.cancelLogin.emit(false);
  }
}
