import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddItemCard } from './add-item-card';

describe('AddItemCard', () => {
  let component: AddItemCard;
  let fixture: ComponentFixture<AddItemCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddItemCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddItemCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
