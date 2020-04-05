import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { CameraComponent } from './camera/camera.component';
import { AuthGuard } from './_guards/auth.guard';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'temperature', component: TemperatureComponent},
            {path: 'camera', component: CameraComponent},
            {path: 'settings', component: SettingsComponent},
            {path: 'profile', component: ProfileComponent},
            {path: 'dashboard', component: DashboardComponent}
        ]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
