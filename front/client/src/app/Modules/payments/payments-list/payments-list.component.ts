import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedParamsDeepLinkingService } from 'src/app/Core/services/paged-params-deep-linking.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';

@Component({
  selector: 'app-payments-list',
  templateUrl: './payments-list.component.html',
  styleUrls: ['./payments-list.component.scss']
})
export class PaymentsListComponent extends BaseComponent implements OnInit {
  private readonly pagedParamsDeepLinkingService: PagedParamsDeepLinkingService;
  pagedListParams: PagedListParams;

  constructor(private readonly route: ActivatedRoute, private readonly router: Router) {
    super();

    this.pagedParamsDeepLinkingService = new PagedParamsDeepLinkingService(route, router);
  }

  ngOnInit(): void {
    this.pagedListParams = this.pagedParamsDeepLinkingService.getPaginatedParams();
  }


}
