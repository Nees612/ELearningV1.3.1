import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAssigmentComponent } from './new-assigment.component';

describe('NewAssigmentComponent', () => {
  let component: NewAssigmentComponent;
  let fixture: ComponentFixture<NewAssigmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAssigmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAssigmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
