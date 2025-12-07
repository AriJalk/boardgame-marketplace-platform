import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RetrieveBggItemId } from './retrieve-bgg-item-id';

describe('RetrieveBggItemId', () => {
  let component: RetrieveBggItemId;
  let fixture: ComponentFixture<RetrieveBggItemId>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RetrieveBggItemId]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RetrieveBggItemId);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
