import { Component, OnInit, OnDestroy, ViewEncapsulation, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ChatMessage, ChatService } from '../../../services/chat/chat.service';
import { AuthService } from '../../../services/Auth/AuthService';
import { BenhNhanService } from '../../../services/benh-nhan/benh-nhan.service';
import { IBenhNhan } from '../../../interfaces/benh-nhan/IBenhNhan';
import { ApiResponse } from '../../../commons/ApiResponse';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-chat.component.html',
  styleUrls: [
    './admin-chat.component.css',
    '/public/assets/admins/css/styles.css',
    '/public/assets/admins/css/custom.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class AdminChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChild('chatBody') chatBody!: ElementRef; // Tham chiếu đến phần tử chat-body

  messages: ChatMessage[] = [];
  newMessage: string = '';
  selectedPatientId: number | null = null;
  maNhanVien: number | null = null;
  patientList: IBenhNhan[] = [];
  chatPatientList: IBenhNhan[] = [];
  activePatients: number[] = [];
  selectedPatientName: string = 'Không xác định';

  constructor(
    private chatService: ChatService,
    private authService: AuthService,
    private benhNhanService: BenhNhanService
  ) {}

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      console.error('User not authenticated');
      return;
    }

    this.maNhanVien = this.authService.getMaTaiKhoanFromToken();
    if (!this.maNhanVien) {
      console.error('Cannot get maTaiKhoan from token');
      return;
    }

    this.loadPatientList();
    this.chatService.messages$.subscribe(messages => {
      console.log('Messages updated in AdminChatComponent:', messages);
      this.messages = messages;
    });

    this.chatService.activePatients$.subscribe(activePatientIds => {
      console.log('Active patients updated:', activePatientIds);
      this.activePatients = activePatientIds;
    });

    this.chatService.chatPatients$.subscribe(chatPatientIds => {
      console.log('Chat patients updated:', chatPatientIds);
      this.chatPatientList = this.patientList.filter(patient => 
        patient.maTaiKhoan !== undefined && chatPatientIds.includes(patient.maTaiKhoan)
      );
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom(); // Cuộn xuống dưới sau mỗi lần cập nhật giao diện
  }

  loadPatientList(): void {
    this.benhNhanService.getAll(1, 1000).subscribe({
      next: (res: ApiResponse<IBenhNhan[]>) => {
        this.patientList = res.status ? res.data || [] : [];
        console.log('Patient list loaded:', this.patientList);
      },
      error: (err: HttpErrorResponse) => {
        this.handleError(err);
      }
    });
  }

  selectPatient(maTaiKhoan: number | undefined) {
    if (this.maNhanVien && maTaiKhoan !== undefined) {
      this.selectedPatientId = maTaiKhoan;
      this.chatService.joinChat(this.maNhanVien, maTaiKhoan);
      const selectedPatient = this.patientList.find(p => p.maTaiKhoan === maTaiKhoan);
      this.selectedPatientName = selectedPatient?.ten || 'Không xác định';
    } else {
      console.warn('Cannot select patient: maTaiKhoan is undefined');
    }
  }

  sendMessage() {
    if (this.newMessage.trim() && this.maNhanVien && this.selectedPatientId !== null) {
      this.chatService.sendMessageToPatient(this.maNhanVien, this.selectedPatientId, this.newMessage);
      this.newMessage = '';
    }
  }

  isPatientActive(maTaiKhoan: number | undefined): boolean {
    return maTaiKhoan !== undefined && this.activePatients.includes(maTaiKhoan);
  }

  private scrollToBottom(): void {
    if (this.chatBody) {
      this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
    }
  }

  private handleError(err: HttpErrorResponse): void {
    console.error('Error loading patient list:', err.message);
  }

  ngOnDestroy() {
    this.chatService.stopConnection();
  }
}