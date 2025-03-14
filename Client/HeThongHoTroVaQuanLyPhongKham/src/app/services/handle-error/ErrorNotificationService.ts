import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ErrorHandlerService } from "./ErrorHandlerService";

@Injectable({
    providedIn: 'root'
  })
export class ErrorNotificationService {
  showNotification: boolean = false;
  messages: string[] = [];
  messageType: 'error' | 'success' | 'info' = 'error';
  private timeoutId: any = null; // Lưu trữ ID của setTimeout

  constructor(private errorHandler: ErrorHandlerService) {}

  public showError(messages: string | string[]): void {
    this.clearTimeoutIfExists(); // Xóa timeout cũ nếu có
    const errorMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = errorMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'error';
    this.showNotification = true;
  }

  public showSuccess(messages: string | string[]): void {
    this.clearTimeoutIfExists(); // Xóa timeout cũ nếu có
    const successMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = successMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'success';
    this.showNotification = true;

    // Tự động đóng sau 3 giây
    this.timeoutId = setTimeout(() => {
      this.closeNotification();
    }, 3000);
  }

  public showInfo(messages: string | string[]): void {
    this.clearTimeoutIfExists(); // Xóa timeout cũ nếu có
    const infoMessages = Array.isArray(messages) ? messages : [messages];
    this.messages = infoMessages.filter(msg => msg && msg.trim() !== '');
    this.messageType = 'info';
    this.showNotification = true;
  }

  public handleError(error: HttpErrorResponse): void {
    this.clearTimeoutIfExists(); // Xóa timeout cũ nếu có
    this.messages = this.errorHandler.handleError(error) || ['Đã xảy ra lỗi không xác định.'];
    this.messageType = 'error';
    this.showNotification = true;
  }

  public showFormValidationErrors(): void {
    this.clearTimeoutIfExists(); // Xóa timeout cũ nếu có
    this.messages = ['Vui lòng điền đầy đủ thông tin'];
    this.messageType = 'error';
    this.showNotification = true;
  }

  public clearNotifications(): void {
    this.showNotification = false;
    this.messages = [];
    this.messageType = 'error';
    this.clearTimeoutIfExists();
  }

  public closeNotification(): void {
    this.clearNotifications();
  }

  private clearTimeoutIfExists(): void {
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
      this.timeoutId = null;
    }
  }
}