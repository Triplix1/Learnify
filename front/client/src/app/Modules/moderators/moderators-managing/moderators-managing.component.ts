import { Component } from '@angular/core';
import { ActivatedRoute, Router, UrlSerializer } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { switchMap, take } from 'rxjs';
import { AuthorizationUseDeepLinkingService } from 'src/app/Core/services/authorization-use-deep-linking.service';
import { ModeratorsDeepLinkingService } from 'src/app/Core/services/moderators-deep-linking.service';
import { ModeratorsService } from 'src/app/Core/services/morderators.service';
import { NavigationService } from 'src/app/Core/services/navigation.service';
import { UserType } from 'src/app/Models/enums/UserType';
import { PagedList } from 'src/app/Models/PagedList';
import { ManagersListParams } from 'src/app/Models/Params/ManagersListParams';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';

@Component({
  selector: 'app-admins-moderators-managing',
  templateUrl: './moderators-managing.component.html',
  styleUrls: ['./moderators-managing.component.scss']
})
export class ModeratorsManagingComponent {
  paginatedModerators: ProfileResponse[];
  totalItems: number = 0;
  moderatorsListParams: ManagersListParams;
  moderatorsDeepLinkingService: ModeratorsDeepLinkingService = new ModeratorsDeepLinkingService(this.route, this.router);
  authorizationUseDeepLinkingService: AuthorizationUseDeepLinkingService = new AuthorizationUseDeepLinkingService(this.router, this.route, this.urlSerializer)

  constructor(private moderatorsService: ModeratorsService,
    private route: ActivatedRoute,
    private router: Router,
    private navigationService: NavigationService,
    private urlSerializer: UrlSerializer,
    private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.navigationService.setupPopstateListener(() => {
      this.navigationService.reloadPage();
    });

    this.moderatorsListParams = this.moderatorsDeepLinkingService.getUserParams();

    this.moderatorsDeepLinkingService.setManagersListParams(this.moderatorsListParams);
    this.fetchData();
  }

  fetchData(): void {
    this.moderatorsService.getList(this.moderatorsListParams).pipe(take(1)).subscribe(
      (data) => {

        if (data) {
          this.paginatedModerators = data.items ?? [];
          this.totalItems = data.totalCount ?? 0;
          this.moderatorsListParams.pageNumber = data.currentPage ?? 1;
          this.moderatorsListParams.pageSize = data.pageSize ?? 10;
        }
      },
    );
  }

  createClicked() {
    this.authorizationUseDeepLinkingService.navigateToRegister(UserType.Moderator);
  }

  onPaginationChange(pagedParams: PagedListParams) {
    this.moderatorsListParams.pageNumber = pagedParams.pageNumber;
    this.moderatorsListParams.pageSize = pagedParams.pageSize;
    this.fetchData();
  }

  deleteModerator(id: number) {
    this.moderatorsService.delete(id).pipe(take(1),
      switchMap(
        deleted => {
          const moderatorsParamsClone = { ...this.moderatorsListParams };


          this.paginatedModerators = this.paginatedModerators?.filter(a => a.id != id);
          this.toastrService.success('Deleted successfully');
          moderatorsParamsClone.pageNumber = moderatorsParamsClone.pageNumber * moderatorsParamsClone.pageSize;
          moderatorsParamsClone.pageSize = 1;
          return this.moderatorsService.getList(moderatorsParamsClone);
        }
      )).subscribe(
        valueToAdd => {
          if (valueToAdd.items && valueToAdd.items.length > 0) {
            this.paginatedModerators.push(valueToAdd.items[0]);
            this.totalItems = valueToAdd.totalCount;
          }
        }
      );
  }

}
