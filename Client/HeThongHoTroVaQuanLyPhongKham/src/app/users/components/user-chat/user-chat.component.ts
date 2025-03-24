import { Component, OnDestroy, OnInit, ViewEncapsulation, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ChatMessage, ChatService } from '../../../services/chat/chat.service';
import { AuthService } from '../../../services/Auth/AuthService';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-chat',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './user-chat.component.html',
  styleUrls: [
    './user-chat.component.css',
    "/public/assets/users/bootstrap/owl.carousel.min.css",
    "/public/assets/users/bootstrap/tempusdominus-bootstrap-4.min.css",
    "/public/assets/users/bootstrap/bootstrap.min.css",
    "/public/assets/users/css/style.css"
  ],
  encapsulation: ViewEncapsulation.None
})
export class UserChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('chatBody') chatBody!: ElementRef;

  messages: ChatMessage[] = [];
  notification: string = '';
  newMessage: string = '';
  maTaiKhoan: number | null = null;

  constructor(private chatService: ChatService, private authService: AuthService) {}

  async ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      console.error('User not authenticated');
      return;
    }

    this.maTaiKhoan = this.authService.getMaTaiKhoanFromToken();
    if (!this.maTaiKhoan) {
      console.error('Cannot get maTaiKhoan from token');
      return;
    }

    // Chờ kết nối SignalR hoàn tất trước khi gọi joinChat
    await new Promise<void>((resolve) => {
      this.chatService.connectionState$.subscribe((isConnected) => {
        if (isConnected) {
          resolve();
        }
      });
    });

    await this.chatService.joinChat(0, this.maTaiKhoan);

    this.chatService.messages$.subscribe(messages => {
      console.log('Messages updated:', messages);
      this.messages = messages;
    });

    this.chatService.notification$.subscribe(notification => {
      console.log('Notification:', notification);
      this.notification = notification;
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  async sendMessage() {
    if (this.newMessage.trim() && this.maTaiKhoan) {
      await this.chatService.sendMessageToStaff(this.maTaiKhoan, this.newMessage);
      this.newMessage = '';
    }
  }

  private scrollToBottom(): void {
    if (this.chatBody) {
      this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
    }
  }

  ngOnDestroy() {
    this.chatService.stopConnection();
  }
}