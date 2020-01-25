import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { CameraComponent } from './camera/camera.component';

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent},
    {path: 'temperature', component: TemperatureComponent},
    {path: 'camera', component: CameraComponent},
    {path: '**', redirectTo: 'home', pathMatch: 'full'},
];
