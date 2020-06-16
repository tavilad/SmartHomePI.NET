import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public emailFormControl = new FormControl('', [
    Validators.email,
  ]);

  constructor() { }

  ngOnInit() {
  }

}
