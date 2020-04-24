import { EventEmitter, Component, OnInit, Input, Output, OnChanges } from '@angular/core';
import { UsersService } from '../services/users.service';
import { ModuleContentsService } from '../services/module-contents.service';
import { IModuleContent } from '../Interfaces/IModuleContent';


@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit, OnChanges {

  @Input() moduleId: number;
  @Output() cancel = new EventEmitter();

  moduleContents: IModuleContent[] = [];
  partNumber: number = 0;

  videoId: string;
  showVideo: boolean = false;

  hasPrevious: boolean;
  hasNext: boolean;

  isUserLoggedIn: boolean;
  isAdminLoggedIn: boolean;

  isEditActive: boolean = false;

  errors: any[] = [];

  constructor(private moduleContentsService: ModuleContentsService, private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.logoutEvent.subscribe(() => {
      this.isUserLoggedIn = false;
    })
    this.refresh();
  }

  ngOnChanges() {
    this.refresh();
  }

  private refresh() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdminLoggedIn = this.usersService.isAdmin()
      this.getContent();
    }, () => {
      this.usersService.raiseLogoutEvent()
      this.isUserLoggedIn = false;
    });
  }

  private getContent() {
    this.moduleContentsService.getModuleContentByModuleId(this.moduleId).subscribe(response => {
      this.moduleContents = response.json();
      this.partNumber = 0;
      this.hasNext = true;
    });
  }

  NextContent() {
    if (this.hasNext) {
      this.partNumber += 1;
      this.hasPrevious = true;
    }
    if (this.partNumber < this.moduleContents.length - 1) {
      this.hasNext = true;
    } else {
      this.hasNext = false;
    }
  }

  PreviousContent() {
    if (this.hasPrevious) {
      this.partNumber -= 1;
      this.hasNext = true;
    }
    if (this.partNumber > 0) {
      this.hasPrevious = true;
    } else {
      this.hasPrevious = false;
    }
  }

  onCancel() {
    this.cancel.emit();
  }

  onVideoPlay() {
    this.showVideo = true;

  }

  onVideoClosed() {

    this.showVideo = false;
  }

  onDelete(moduleContentId) {
    this.moduleContentsService.deleteModuleContent(moduleContentId).subscribe(() => {
      this.onCancel();
    })
  }

  onEditCancel() {
    this.errors = [];
    this.isEditActive = false;
    this.getContent();
  }

  onEdit() {
    this.isEditActive = true;
  }

  onSave(id) {
    this.errors = [];
    let moduleContent = {
      Title: this.moduleContents[this.partNumber].title,
      Description: this.moduleContents[this.partNumber].description,
      AssigmentUrl: this.moduleContents[this.partNumber].assigmentUrl,
      Lesson: this.moduleContents[this.partNumber].lesson
    }
    this.moduleContentsService.updateModuleContent(moduleContent, id).subscribe(() => {
      this.getContent();
      this.isEditActive = false;
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }

}
