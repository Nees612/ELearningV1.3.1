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
  selectedContent: any;
  safeUrl: SafeResourceUrl;
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
      this.selectedContent = this.moduleContents[0];
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.selectedContent.tutorialUrl);
      this.hasNext = true;
    }, _error => {
      alert('Your login has expeired please login again.');
    })
  }


  NextContent() {
    this.partNumber += 1;
    this.selectedContent = this.moduleContents[this.partNumber];
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
    this.selectedContent = this.moduleContents[this.partNumber];
    this.Change();
    this.hasNext = true;
    if (this.partNumber > 0) {
      this.hasPrevious = true;
    } else {
      this.hasPrevious = false;
    }
  }

  Change() {
    if (this.selectedContent.tutorialUrl !== null) {
      this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.selectedContent.tutorialUrl);
    } else {
      this.safeUrl = null;
    }
  }

  onCancel() {
    this.cancel.emit();
  }
}
