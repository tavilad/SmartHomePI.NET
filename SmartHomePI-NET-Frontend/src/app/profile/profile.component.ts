import { Component, OnInit } from "@angular/core";
import { FormControl, Validators, ReactiveFormsModule } from "@angular/forms";
import { AuthService } from "../_services/auth.service";
import { CrudService } from "../_services/CRUD.service";
import { AlertifyService } from "../_services/alertify.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"],
})
export class ProfileComponent implements OnInit {
  public emailFormControl = new FormControl("", [
    Validators.email,
    Validators.required,
  ]);

  model: any = {};

  constructor(
    private crudService: CrudService,
    private authService: AuthService,
    private alertyfyService: AlertifyService
  ) {}

  ngOnInit() {}

  createUserDetails() {
    // tslint:disable-next-line: radix
    this.model.userId = parseInt(this.authService.decodedToken.nameid);
    this.crudService.create(this.model, "userdetails").subscribe(() => {
      console.log(this.model);
      this.alertyfyService.success("User details saved");
    });
  }
}
