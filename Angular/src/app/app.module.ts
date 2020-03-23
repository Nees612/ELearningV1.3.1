import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { ModuleComponent } from './module/module.component';
import { OtherUsersComponent } from './other-users/other-users.component';
import { AssigmentsComponent } from './assigments/assigments.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { CookieService } from 'ngx-cookie-service';
import { ModulesService } from './services/modules.service';
import { AssigmentsService } from './services/assigments.service';
import { RouterModule } from '@angular/router';
import { UsersService } from './services/users.service';
import { HeadersService } from './services/headers.service';
import { OtherUsersProfileComponent } from './other-users-profile/other-users-profile.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    RegistrationComponent,
    LoginComponent,
    ModuleComponent,
    OtherUsersComponent,
    OtherUsersProfileComponent,
    AssigmentsComponent,
    MyProfileComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'assigments', component: AssigmentsComponent },
      { path: 'all_profiles', component: OtherUsersComponent },
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
      { path: 'my_profile', component: MyProfileComponent },
      { path: 'profile', component: OtherUsersProfileComponent },
      { path: 'profile/:userName', component: OtherUsersProfileComponent }
    ])
  ],
  providers: [CookieService, ModulesService, AssigmentsService, UsersService, HeadersService],
  bootstrap: [AppComponent]
})
export class AppModule { }
