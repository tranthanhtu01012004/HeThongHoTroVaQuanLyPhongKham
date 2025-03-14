import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ErrorHandlerService } from "../../commons/ErrorHandlerService";

@Injectable({
    providedIn: 'root'
  })
export class ErrorNotificationService {
constructor(private errorHandler: ErrorHandlerService) { }
showNotification: boolean = false;
errorMessages: string[] = [];

public handleError(error: HttpErrorResponse): void {
    this.errorMessages = this.errorHandler.handleError(error);
    this.showNotification = true;
}

public showFormValidationErrors(): void {
    this.errorMessages = ['Vui lòng điền đầy đủ thông tin'];
    this.showNotification = true;
}

public clearNotifications(): void {
    this.showNotification = false;
    this.errorMessages = [];
}

public closeNotification(): void {
    this.clearNotifications();
}
}