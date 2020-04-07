import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewModuleContentComponent } from './new-module-content.component';

describe('NewModuleContentComponent', () => {
  let component: NewModuleContentComponent;
  let fixture: ComponentFixture<NewModuleContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewModuleContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewModuleContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
