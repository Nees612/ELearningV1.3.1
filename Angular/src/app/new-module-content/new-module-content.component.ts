import { Component, OnInit } from '@angular/core';
import { ModuleContentsService } from '../services/module-contents.service';
import { UsersService } from '../services/users.service';
import { ModulesService } from '../services/modules.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-module-content',
  templateUrl: './new-module-content.component.html',
  styleUrls: ['./new-module-content.component.css']
})
export class NewModuleContentComponent implements OnInit {

  constructor(private moduleContentsService: ModuleContentsService, private usersService: UsersService, private modulesService: ModulesService, private router: Router) { }

  title: string;
  description: string;
  assigmentUrl: string;
  lesson: string;

  modules: any[];
  selectedModuleId: number = 1;

  errors: any[] = [];

  isAdminLoggedIn: boolean = false;

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      if (this.usersService.isAdmin) {
        this.isAdminLoggedIn = true;
        this.modulesService.getAllModules().subscribe(response => {
          this.modules = response.json();
        })
      }
    })
  }

  onSubmit() {
    this.errors = [];
    let moduleContent = {
      ModuleId: +this.selectedModuleId,
      Title: this.title,
      Description: this.description,
      AssigmentUrl: this.assigmentUrl,
      Lesson: this.lesson
    }
    this.moduleContentsService.addModuleContent(moduleContent).subscribe(() => {
      alert('New Module contents succesfully added !');
      this.router.navigate['/home'];
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }
}

