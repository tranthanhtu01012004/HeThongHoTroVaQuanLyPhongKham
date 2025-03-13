import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class LoginStore {
    private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
    isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

    setAuthenticated(status: boolean): void {
        this.isAuthenticatedSubject.next(status);
    }
}