import { Component, OnInit } from '@angular/core';
import { VideosService } from '../services/videos.service';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-new-video',
  templateUrl: './new-video.component.html',
  styleUrls: ['./new-video.component.css']
})
export class NewVideoComponent implements OnInit {

  title: string;
  description: string;
  url: string;

  moduleContents: any[];

  selectedContentId: number = 1;

  errors: any[] = [];

  isuserLoggedIn: boolean = false;

  constructor(private videosService: VideosService, private modulesService: ModulesService, private usersService: UsersService) { }

  ngOnInit() {
    if (this.usersService.isUserLoggedIn() && this.usersService.isAdmin()) {
      this.isuserLoggedIn = true;
      this.modulesService.getAllModuleContents().subscribe(response => {
        this.moduleContents = response.json().contents;
      })
    }
  }

  onSubmit() {
    this.errors = [];
    let video = {
      Title: this.title,
      Description: this.description,
      Url: this.url,
      ModuleContentId: +this.selectedContentId
    }
    this.videosService.addVideo(video).subscribe(() => {
      alert('New video succesfully added.');
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }
}
