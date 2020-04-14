import { Component, OnInit } from '@angular/core';
import { VideosService } from '../services/videos.service';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';
import { ModuleContentsService } from '../services/module-contents.service';
import { IModuleContent } from '../Interfaces/IModuleContent';
import { INewVideo } from '../Interfaces/INewVideo';

@Component({
  selector: 'app-new-video',
  templateUrl: './new-video.component.html',
  styleUrls: ['./new-video.component.css']
})
export class NewVideoComponent implements OnInit {


  moduleContents: IModuleContent[];

  selectedContentId: number = 1;

  newVideo: INewVideo = { Title: '', Description: '', Url: '', ModuleContentId: this.selectedContentId }

  errors: any[] = [];

  isAdminLoggedIn: boolean = false;

  constructor(private videosService: VideosService, private moduleContentsService: ModuleContentsService, private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      if (this.usersService.isAdmin()) {
        this.isAdminLoggedIn = true;
        this.moduleContentsService.getAllModuleContents().subscribe(response => {
          this.moduleContents = response.json();
        });
      }
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isAdminLoggedIn = false;
    });
  }

  onSubmit() {
    this.errors = [];
    this.videosService.addVideo(this.newVideo).subscribe(() => {
      alert('New video succesfully added.');
      this.router.navigate(['/home']);
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }
}
