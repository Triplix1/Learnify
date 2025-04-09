import { Component } from '@angular/core';
import { ActivatedRoute, Router, UrlSerializer } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { switchMap, take } from 'rxjs';
import { AdminsDeepLinkingService } from 'src/app/Core/services/admins-deep-linking.service';
import { AdminsService } from 'src/app/Core/services/admins.service';
import { AuthorizationUseDeepLinkingService } from 'src/app/Core/services/authorization-use-deep-linking.service';
import { NavigationService } from 'src/app/Core/services/navigation.service';
import { UserType } from 'src/app/Models/enums/UserType';
import { AdminsListParams } from 'src/app/Models/Params/AdminsListParams';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';

@Component({
  selector: 'app-admins-moderators-managing',
  templateUrl: './admins-managing.component.html',
  styleUrls: ['./admins-managing.component.scss']
})
export class AdminsManagingComponent {
  paginatedAdmins: ProfileResponse[];
  totalItems: number = 0;
  adminsListParams: AdminsListParams;
  adminsDeepLinkingService: AdminsDeepLinkingService = new AdminsDeepLinkingService(this.route, this.router);
  authorizationUseDeepLinkingService: AuthorizationUseDeepLinkingService = new AuthorizationUseDeepLinkingService(this.router, this.route, this.urlSerializer)

  constructor(private readonly adminsService: AdminsService,
    private route: ActivatedRoute,
    private router: Router,
    private navigationService: NavigationService,
    private urlSerializer: UrlSerializer,
    private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.navigationService.setupPopstateListener(() => {
      this.navigationService.reloadPage();
    });

    this.adminsListParams = this.adminsDeepLinkingService.getUserParams();

    this.adminsDeepLinkingService.setManagersListParams(this.adminsListParams);
    this.fetchData();
  }

  fetchData(): void {
    this.adminsService.getList(this.adminsListParams).pipe(take(1)).subscribe(
      (data) => {

        if (data) {
          this.paginatedAdmins = data.items ?? [];
          this.totalItems = data.totalCount ?? 0;
          this.adminsListParams.pageNumber = data.currentPage ?? 1;
          this.adminsListParams.pageSize = data.pageSize ?? 10;
        }
      },
    );
  }

  createClicked() {
    this.authorizationUseDeepLinkingService.navigateToRegister(UserType.Admin);
  }

  onPaginationChange(pagedParams: PagedListParams) {
    this.adminsListParams.pageNumber = pagedParams.pageNumber;
    this.adminsListParams.pageSize = pagedParams.pageSize;
    this.fetchData();
  }

  deleteModerator(id: number) {
    this.adminsService.delete(id).pipe(take(1),
      switchMap(
        deleted => {
          const moderatorsParamsClone = { ...this.adminsListParams };

          this.paginatedAdmins = this.paginatedAdmins?.filter(a => a.id != id);
          this.toastrService.success('Deleted successfully');
          moderatorsParamsClone.pageNumber = moderatorsParamsClone.pageNumber * moderatorsParamsClone.pageSize;
          moderatorsParamsClone.pageSize = 1;
          return this.adminsService.getList(moderatorsParamsClone);
        }
      )).subscribe(
        valueToAdd => {
          if (valueToAdd.items && valueToAdd.items.length > 0) {
            this.paginatedAdmins.push(valueToAdd.items[0]);
            this.totalItems = valueToAdd.totalCount;
          }
        }
      );
  }
}
