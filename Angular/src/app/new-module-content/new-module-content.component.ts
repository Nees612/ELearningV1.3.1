import { Component, OnInit } from '@angular/core';
import { ModuleContentsService } from '../services/module-contents.service';
import { UsersService } from '../services/users.service';
import { ModulesService } from '../services/modules.service';
import { Router } from '@angular/router';
import { IModule } from '../Interfaces/IModule';
import { INewModuleContent } from '../Interfaces/INewModuleContent';

@Component({
  selector: 'app-new-module-content',
  templateUrl: './new-module-content.component.html',
  styleUrls: ['./new-module-content.component.css']
})
export class NewModuleContentComponent implements OnInit {


  modules: IModule[];
  selectedModuleId: number = 1;

  newModuleContent: INewModuleContent = { Title: '', Description: '', AssigmentUrl: null, Lesson: '', ModuleId: this.selectedModuleId }

  errors: any[] = [];

  isAdminLoggedIn: boolean = false;

  constructor(private moduleContentsService: ModuleContentsService, private usersService: UsersService, private modulesService: ModulesService, private router: Router) { }

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
    this.moduleContentsService.addModuleContent(this.newModuleContent).subscribe(() => {
      alert('New Module contents succesfully added !');
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

