import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { ErrorNotificationService } from '../../../services/handle-error/ErrorNotificationService';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrls: [
    './notification.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class NotificationComponent {
  constructor(public notificationService: ErrorNotificationService) {}

  get message(): string {
    return this.notificationService.messages.join(', ');
  }

  get isVisible(): boolean {
    return this.notificationService.showNotification;
  }

  get alertClass(): string {
    switch (this.notificationService.messageType) {
      case 'success':
        return 'alert-success';
      case 'info':
        return 'alert-info';
      case 'error':
      default:
        return 'alert-danger';
    }
  }

  onClose(): void {
    this.notificationService.closeNotification();
  }
}
