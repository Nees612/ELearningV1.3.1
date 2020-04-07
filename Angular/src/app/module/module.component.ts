import { EventEmitter, Component, OnInit, Input, Output } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';
import { ModuleContentsService } from '../services/module-contents.service';


@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit {

  @Input() moduleId: number;
  @Output() cancel = new EventEmitter();

  moduleContents: any[];
  partNumber: number;
  safeUrl: SafeResourceUrl;

  videoId: string;
  showVideo: boolean = false;

  hasPrevious: boolean;
  hasNext: boolean;

  isUserLoggedIn: boolean;


  constructor(private moduleContentsService: ModuleContentsService, private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.logoutEvent.subscribe(() => {
      this.isUserLoggedIn = false;
    })
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
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
    this.partNumber += 1;
    this.hasPrevious = true;
    if (this.partNumber < this.moduleContents.length - 1) {
      this.hasNext = true;
    } else {
      this.hasNext = false;
    }
  }

  PreviousContent() {
    this.partNumber -= 1;
    this.hasNext = true;
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


}
