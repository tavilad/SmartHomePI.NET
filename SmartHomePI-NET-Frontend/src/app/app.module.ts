import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';

import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { TemperatureComponent } from './temperature/temperature.component';
import { CameraComponent } from './camera/camera.component';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { DashboardComponent, DialogComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { CrudService } from './_services/CRUD.service';
import { RoompageComponent } from './roompage/roompage.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      TemperatureComponent,
      CameraComponent,
      SettingsComponent,
      ProfileComponent,
      DashboardComponent,
      LoginComponent,
      DialogComponent,
      RoompageComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      MatInputModule,
      MatTableModule,
      MatPaginatorModule,
      MatSortModule,
      MatIconModule,
      MatFormFieldModule,
      MatButtonModule,
      MatMenuModule,
      MatDividerModule,
      MatGridListModule,
      MatDialogModule,
      MatCardModule,
      ReactiveFormsModule,
      ChartsModule,
      MatSlideToggleModule
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      CrudService
   ],
   bootstrap: [
      AppComponent
   ],
})
export class AppModule { }
