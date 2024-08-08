import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowcaseButtonsComponent } from './showcase-buttons.component';

describe('ShowcaseButtonsComponent', () => {
  let component: ShowcaseButtonsComponent;
  let fixture: ComponentFixture<ShowcaseButtonsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ShowcaseButtonsComponent]
    });
    fixture = TestBed.createComponent(ShowcaseButtonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
