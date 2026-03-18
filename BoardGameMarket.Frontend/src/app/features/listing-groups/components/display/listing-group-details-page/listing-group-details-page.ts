import { Component, OnInit } from '@angular/core';
import { ListingGroupDetails } from '../../../models/listing-group.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { ListingGroupService } from '../../../services/listing-group.service';
import { ListingGroupDetailsCard } from '../listing-group-details-card/listing-group-details-card';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { UserSummary } from '../../../../../core/users/models/user.model';
import { EditListingGroupPage } from '../../edit-listing-group-page/edit-listing-group-page';
import { UsersService } from '../../../../users/services/users-service';
import { AuthService } from '../../../../../core/auth/auth.service';
import { AccountInfo } from '@azure/msal-browser';

@Component({
  selector: 'bgm-listing-group-details-page',
  imports: [MatCardModule, ListingGroupDetailsCard, AsyncPipe, EditListingGroupPage, RouterLink],
  templateUrl: './listing-group-details-page.html',
  styleUrl: './listing-group-details-page.css',
})
export class ListingGroupDetailsPage implements OnInit {
  listingGroup$?: Observable<ListingGroupDetails>;
  user$?: Observable<AccountInfo | null>;

  constructor(
    private router: ActivatedRoute,
    private listingGroupService: ListingGroupService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    this.router.paramMap.subscribe((params) => {
      const id = params.get('listingGroupId');
      if (id) {
        // this.listingGroupService.getListingGroupDetails(id).subscribe((data) => console.log(data));
        this.user$ = this.authService.activeAccount$;
        this.listingGroup$ = this.authService.withOptionalAuthCall((header) => {
          return (this.listingGroup$ = this.listingGroupService.getListingGroupDetails(
            id,
            header ?? undefined,
          ));
        });
      }
    });
  }
}
