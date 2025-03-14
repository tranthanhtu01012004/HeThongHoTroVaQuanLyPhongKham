import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ErrorHandlerService } from "./ErrorHandlerService";

@Injectable({
    providedIn: 'root'
  })
export class NotificationService {
  showNotification: boolean = false;
  messages: string[] = [];
  messageType: 'error' | 'success' | 'info' = 'error'; // Định nghĩa kiểu union cho messageType
  private timeoutId: any = null;
  private returnUrl: string | null = null;
  private confirmationCallback: ((confirmed: boolean) => void) | null = null;

  constructor(private errorHandler: ErrorHandlerService) {}

  public showError(messages: string | string[]): void {
    this.clearTimeoutIfExists();
    const errorMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = errorMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'error';
    this.showNotification = true;
  }

  public showSuccess(messages: string | string[]): void {
    this.clearTimeoutIfExists();
    const successMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = successMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'success';
    this.showNotification = true;

    this.timeoutId = setTimeout(() => {
      this.closeNotification();
    }, 3000);
  }

  public showInfo(messages: string | string[], confirmable: boolean = false): void {
    this.clearTimeoutIfExists();
    const infoMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = infoMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'info';
    this.showNotification = true;

    if (confirmable) {
      this.confirmationCallback = (confirmed: boolean) => {
        if (confirmed) {
          this.navigateToLogin();
        }
        this.closeNotification();
      };
    }
  }

  public handleError(error: HttpErrorResponse): void {
    this.clearTimeoutIfExists();
    this.messages = this.errorHandler.handleError(error) || ['Đã xảy ra lỗi không xác định.'];
    this.messageType = 'error';
    this.showNotification = true;
  }

  public showFormValidationErrors(): void {
    this.clearTimeoutIfExists();
    this.messages = ['Vui lòng điền đầy đủ thông tin'];
    this.messageType = 'error';
    this.showNotification = true;
  }

  public clearNotifications(): void {
    this.showNotification = false;
    this.messages = [];
    this.messageType = 'error';
    this.clearTimeoutIfExists();
    this.confirmationCallback = null;
  }

  public closeNotification(confirmed: boolean = false): void {
    if (this.confirmationCallback) {
      this.confirmationCallback(confirmed);
    }
    this.clearNotifications();
  }

  public setReturnUrl(url: string): void {
    this.returnUrl = url;
  }

  private navigateToLogin(): void {
    if (this.returnUrl) {
      window.location.href = `/dang-nhap?returnUrl=${encodeURIComponent(this.returnUrl)}`;
    } else {
      window.location.href = '/dang-nhap';
    }
  }

  private clearTimeoutIfExists(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }
  }
}