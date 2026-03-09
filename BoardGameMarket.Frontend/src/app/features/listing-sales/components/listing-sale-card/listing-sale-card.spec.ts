import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListingSaleCard } from './listing-sale-card';

describe('ListingSaleCard', () => {
  let component: ListingSaleCard;
  let fixture: ComponentFixture<ListingSaleCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListingSaleCard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListingSaleCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
