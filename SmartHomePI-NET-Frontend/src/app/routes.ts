import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { CameraComponent } from './camera/camera.component';
import { AuthGuard } from './_guards/auth.guard';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FormFieldAppearanceExampleComponent } from './FormFieldAppearanceExample/FormFieldAppearanceExample.component';

export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'temperature', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: TemperatureComponent},
    {path: 'camera', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: CameraComponent},
    {path: 'settings', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: SettingsComponent},
    {path: 'profile', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: ProfileComponent},
    {path: 'dashboard', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: DashboardComponent},
    {path: 'example', runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard], component: FormFieldAppearanceExampleComponent},
    {path: '**', redirectTo: '', pathMatch: 'full'},
];
