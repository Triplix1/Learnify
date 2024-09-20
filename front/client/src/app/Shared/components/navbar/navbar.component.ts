import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, UrlSerializer, UrlTree } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { MatIconRegistry } from '@angular/material/icon';
import { AuthService } from 'src/app/Core/services/auth.service';
import { DropdownItem } from 'src/app/Models/DropdownItem';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  dropdownItems: DropdownItem[] = [
    { itemText: "Профіль", link: "/profile" },
    { itemText: "Вийти", action: () => this.logout() },
    { itemText: "Чати", link: "/messages/join" },
  ]

  Theme: string = "theme";
  isDarkMode: boolean = localStorage.getItem(this.Theme) === 'true';
  @Output() switchedTheme: EventEmitter<boolean> = new EventEmitter();
  tokenData = this.accountService.tokenData$;
  isAdmin: boolean = false;
  searchValue: string | null = null;
  private currentRoute: string | undefined;
  private subscriptions: Subscription[] = [];

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
    this.switchedTheme.emit(this.isDarkMode);
  }

  login() {
    this.router.navigate(['/login']);
  }

  logout() {
    this.accountService.logout();
  }

  switchTheme() {
    this.isDarkMode = !this.isDarkMode;
    localStorage.setItem(this.Theme, this.isDarkMode.toString());
    this.switchedTheme.emit(this.isDarkMode);
  }
}
