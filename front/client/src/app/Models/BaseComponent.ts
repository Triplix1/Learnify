import { Component, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";

@Component({
    template: ''
})
export class BaseComponent implements OnDestroy {
    destroySubject: Subject<void> = new Subject<void>();

    reinitialize(): void {
        this.destroy();

    }

    ngOnDestroy(): void {
        this.destroy();
        this.destroySubject = new Subject<void>();
    }

    private destroy() {
        this.destroySubject.next();
        this.destroySubject.complete();
    }
}
