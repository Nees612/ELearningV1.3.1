import { EventEmitter, Component, OnInit, Input, Output } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ModulesService } from '../services/modules.service';


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


  constructor(private modulesService: ModulesService) { }

  ngOnInit() {
    this.getContent();
  }


  private getContent() {
    this.modulesService.getModuleContentByModuleId(this.moduleId).subscribe(response => {
      this.moduleContents = response.json().moduleContents;
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
