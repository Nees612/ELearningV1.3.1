import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeContentOrderComponent } from './change-content-order.component';

describe('ChangeContentOrderComponent', () => {
  let component: ChangeContentOrderComponent;
  let fixture: ComponentFixture<ChangeContentOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeContentOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeContentOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
