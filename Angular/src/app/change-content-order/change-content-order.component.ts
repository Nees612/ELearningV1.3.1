import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { ModuleContentsService } from '../services/module-contents.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-change-content-order',
  templateUrl: './change-content-order.component.html',
  styleUrls: ['./change-content-order.component.css']
})
export class ChangeContentOrderComponent implements OnInit, OnChanges {

  @Input() moduleId: number;
  @Output() cancel = new EventEmitter()

  moduleContents: any[] = [];

  constructor(private moduleContentsService: ModuleContentsService, private usersService: UsersService) { }

  ngOnInit() {
    this.refresh();
  }

  ngOnChanges() {
    this.refresh()
  }

  private refresh() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.moduleContentsService.getModuleContentByModuleId(this.moduleId).subscribe(response => {
        this.moduleContents = response.json();
      })
    })
  }

  onSave() {
    for (let moduleContent of this.moduleContents) {
      this.moduleContentsService.changeModuleContentContentId(moduleContent.id, moduleContent.contentId).subscribe();
    }
    alert('Order changed succesfully.');
    this.onCancel();
  }

  onCancel() {
    this.cancel.emit();
  }

}
