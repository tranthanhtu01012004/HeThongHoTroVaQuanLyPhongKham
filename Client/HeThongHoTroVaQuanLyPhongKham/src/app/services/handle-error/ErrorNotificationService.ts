import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ErrorHandlerService } from "./ErrorHandlerService";

@Injectable({
    providedIn: 'root'
  })
export class ErrorNotificationService {
showNotification: boolean = false;
errorMessages: string[] = [];

constructor(private errorHandler: ErrorHandlerService) { }

/**
   * Xử lý lỗi HTTP và hiển thị thông báo
   * @param error Đối tượng HttpErrorResponse từ Angular HttpClient
*/
public handleError(error: HttpErrorResponse): void {
    this.errorMessages = this.errorHandler.handleError(error) || ['Đã xảy ra lỗi không xác định.'];
    this.showNotification = true;
  }

/**
   * Hiển thị thông báo lỗi khi form không hợp lệ
*/
public showFormValidationErrors(): void {
    this.errorMessages = ['Vui lòng điền đầy đủ thông tin'];
    this.showNotification = true;
  }

/**
   * Hiển thị thông báo lỗi tùy chỉnh
   * @param messages Mảng thông báo lỗi hoặc chuỗi đơn
*/
public handleCustomError(messages: string | string[]): void {
// Chuyển thành mảng nếu là chuỗi đơn
const errorMessages = Array.isArray(messages) ? messages : [messages];
this.errorMessages = errorMessages.filter(msg => msg && msg.trim() !== '');
this.showNotification = true;
}

/**
    * Xóa tất cả thông báo và ẩn thông báo
*/
public clearNotifications(): void {
    this.showNotification = false;
    this.errorMessages = [];
}

/**
    * Đóng thông báo (gọi clearNotifications)
 */
public closeNotification(): void {
    this.clearNotifications();
}
}