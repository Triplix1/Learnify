import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSerializer, UrlTree } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { MatIconRegistry } from '@angular/material/icon';
import { AuthService } from 'src/app/Core/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isAuthorized = this.accountService.authNavStatus$;
  isAdmin: boolean = false;
  searchValue: string | null = null;
  private currentRoute: string | undefined;
  private subscriptions: Subscription[] = []

  get searchIcon() {
    return `<span class="input-group-text border-0 bg-opacity-0"
    id="search-addon">
  <i class="fa fa-search text-white"
     style="color: #ffffff;"></i>
</span>`
  }

  constructor(private accountService: AuthService, private router: Router, private urlSerializer: UrlSerializer, private route: ActivatedRoute, private matIconRegistry: MatIconRegistry) {
    matIconRegistry.addSvgIconLiteral("search", `<span class="input-group-text border-0 bg-opacity-0"
    id="search-addon">
  <i class="fa fa-search text-white"
     style="color: #ffffff;"></i>
</span>`)
  }

  ngOnInit(): void {
    // this.router.events
    //   .pipe(filter(event => event instanceof NavigationEnd))
    //   .subscribe(() => {
    //     this.currentRoute = this.getCurrentRoute(this.route.root);
    //     //this.searchValue = this.navbarService.loadSearchField();
    //   });
  }

  // search() {
  //   this.router.navigate(["/list"], {
  //     queryParams: {
  //       search: this.searchValue,
  //     },
  //     replaceUrl: true
  //   })
  // }

  // private getCurrentRoute(route: ActivatedRoute): string {
  //   while (route.firstChild) {
  //     route = route.firstChild;
  //   }
  //   return route.snapshot.url.map(segment => segment.path).join('/');
  // }

  login() {
    this.accountService.login();
  }

  logout() {
    this.accountService.logout();
  }
}
