import { EventEmitter, Component, OnInit, Input, OnChanges, Output } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ModulesService } from '../services/modules.service';


@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit, OnChanges {

  @Input() moduleId: number;
  @Output() cancel = new EventEmitter();

  moduleContents: any[];
  partNumber: number;
  safeUrl: SafeResourceUrl;

  videoId: string;
  showVideo: boolean = false;

  hasPrevious: boolean;
  hasNext: boolean;


  constructor(private modulesService: ModulesService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.getContent();
  }

  ngOnChanges() {
    this.getContent();
  }

  private getContent() {
    this.modulesService.getModuleContentByModuleId(this.moduleId).subscribe(response => {
      this.moduleContents = response.json().moduleContents;
      this.partNumber = this.moduleContents[0].contentId;
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.moduleContents[0].tutorialUrl);
      this.videoId = this.moduleContents[0].tutorialUrl.split('/').slice(-1)[0];
      this.hasNext = true;
    }, _error => {
      alert('Your login has expeired please login again.');
    })
  }


  NextContent() {
    this.partNumber += 1;
    this.Change();
    this.hasPrevious = true;
    if (this.partNumber < this.moduleContents.length - 1) {
      this.hasNext = true;
    } else {
      this.hasNext = false;
    }
  }

  PreviousContent() {
    this.partNumber -= 1;
    this.Change();
    this.hasNext = true;
    if (this.partNumber > 0) {
      this.hasPrevious = true;
    } else {
      this.hasPrevious = false;
    }
  }

  Change() {
    if (this.moduleContents[this.partNumber].tutorialUrl !== null) {
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.moduleContents[this.partNumber].tutorialUrl);
      this.videoId = this.moduleContents[this.partNumber].tutorialUrl.split('/').slice(-1)[0];
    } else {
      this.safeUrl = null;
    }
  }

  onCancel() {
    this.cancel.emit();
  }

  onVideoClick() {
    this.showVideo = true;
  }

  onVideoClosed() {
    this.showVideo = false;
  }
}
