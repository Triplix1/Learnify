import { Component, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";

@Component({
    template: ''
})
export class BaseComponent implements OnDestroy {
    destroySubject: Subject<void> = new Subject<void>();

    ngOnDestroy(): void {
        this.destroySubject.next();
        this.destroySubject.complete();
    }

}
