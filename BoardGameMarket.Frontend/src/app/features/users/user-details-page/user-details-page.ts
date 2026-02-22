import { Component, Inject, Input, OnInit } from '@angular/core';
import { UserSummary } from '../../../core/users/models/user.model';
import { HttpClient } from '@angular/common/http';
import { AuthStubService } from '../../../core/auth/auth-stub.service';
import { API_CONFIG_SERVICE } from '../../../core/api-config/api-config.service';
import { ApiConfig } from '../../../core/api-config/api-config.interface';
import { UsersService } from '../services/users-service';
import { MatAnchor, MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'bgm-user-details-page',
  imports: [MatButtonModule, AsyncPipe],
  templateUrl: './user-details-page.html',
  styleUrl: './user-details-page.css',
})
export class UserDetailsPage implements OnInit {
  userId!: string;
  user$!: Observable<UserSummary>;
  contactInfo$?: Observable<string>;

  constructor(
    private router: ActivatedRoute,
    private usersService: UsersService,
  ) {}

  ngOnInit(): void {
    this.router.paramMap.subscribe((params) => {
      const id = params.get('userId');
      if (id) {
        this.user$ = this.usersService.getUserSummary(id);
        this.userId = id;
      }
    });
  }

  onContactDetailsRequested() {
    this.contactInfo$ = this.usersService.getContactInfo(this.userId);
  }
}
