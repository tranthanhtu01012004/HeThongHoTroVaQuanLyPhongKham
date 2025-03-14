import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class LoginStore {
    private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
    private roleSubject = new BehaviorSubject<string>('');

    isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
    role$ = this.roleSubject.asObservable();
    
    setAuthenticated(status: boolean): void {
        this.isAuthenticatedSubject.next(status);
    }

    setRole(role: string): void {
        this.roleSubject.next(role);
      }
}