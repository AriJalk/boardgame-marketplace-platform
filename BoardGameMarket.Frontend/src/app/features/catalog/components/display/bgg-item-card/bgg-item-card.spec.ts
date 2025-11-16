import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BggItemCard } from './bgg-item-card';

describe('BggItemCard', () => {
  let component: BggItemCard;
  let fixture: ComponentFixture<BggItemCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BggItemCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BggItemCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
