import { OnDestroy } from "@angular/core";
import { Subject } from "rxjs";

export class BaseComponent implements OnDestroy {
    destroySubject: Subject<void> = new Subject<void>();

    ngOnDestroy(): void {
        throw new Error("Method not implemented.");
    }

}