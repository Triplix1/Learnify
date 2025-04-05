import { Injectable } from '@angular/core';
import { ActivatedRoute, Router, UrlSerializer, UrlTree } from '@angular/router';
import { UserType } from 'src/app/Models/enums/UserType';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationUseDeepLinkingService {

  constructor(private router: Router, private route: ActivatedRoute, private urlSerializer: UrlSerializer) { }

  navigateToLogin() {
    this.router.navigate(["login"], {
      queryParams: {
        returnUrl: this.getReturnUrl(),
      },
      queryParamsHandling: 'merge',
      skipLocationChange: true
    });
  }

  navigateToRegister(userType: UserType = null) {
    var navigateUrl = "register"

    if (userType) {
      navigateUrl += "/" + userType;
    }

    this.router.navigate([`register/${userType}`], {
      queryParams: {
        returnUrl: this.getReturnUrl(),
      },
      queryParamsHandling: 'merge',
      skipLocationChange: false
    });
  }

  getReturnUrl() {
    const urlTree: UrlTree = this.router.createUrlTree([], { relativeTo: this.router.routerState.root });
    const returnUrl = this.urlSerializer.serialize(urlTree);
    return returnUrl;
  }
}
