import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { QuestionsComponent } from './questions/questions.component';
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

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    QuestionsComponent,
    NavbarComponent,
    RegistrationComponent,
    LoginComponent,
    ModuleComponent,
    OtherUsersComponent,
    AssigmentsComponent,
    MyProfileComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpModule,
    FormsModule
  ],
  providers: [CookieService, ModulesService, AssigmentsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
