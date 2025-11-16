import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomItemCard } from './custom-item-card';

describe('CustomItemCard', () => {
  let component: CustomItemCard;
  let fixture: ComponentFixture<CustomItemCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomItemCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomItemCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
