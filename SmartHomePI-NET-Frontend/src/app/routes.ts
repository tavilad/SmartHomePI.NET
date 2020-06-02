import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { CameraComponent } from './camera/camera.component';
import { AuthGuard } from './_guards/auth.guard';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RoompageComponent } from './roompage/roompage.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'temperature', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: TemperatureComponent, data: {animation: 'TemperaturePage'}},
    {path: 'camera', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: CameraComponent, data: {animation: 'CameraPage'}},
    {path: 'settings', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: SettingsComponent, data: {animation: 'SettingsPage'}},
    {path: 'profile', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: ProfileComponent, data: {animation: 'ProfilePage'}},
    {path: 'dashboard', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: DashboardComponent, data: {animation: 'DashboardPage'}},
    {path: 'roompage/:id', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: RoompageComponent, data: {animation: 'RoomPage'}},
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
