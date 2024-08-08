import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataPageComponent } from './data-page.component';

describe('DataPageComponent', () => {
  let component: DataPageComponent;
  let fixture: ComponentFixture<DataPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DataPageComponent]
    });
    fixture = TestBed.createComponent(DataPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
