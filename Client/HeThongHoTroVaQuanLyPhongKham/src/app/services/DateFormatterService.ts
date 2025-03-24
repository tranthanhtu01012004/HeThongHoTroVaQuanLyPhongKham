import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})

export class DateFormatterService {
  /**
   * Định dạng ngày giờ thành chuỗi ISO 8601 với định dạng "YYYY-MM-DDTHH:mm:ss"
   */
  formatToISOString(date: Date | string): string {
    const d = new Date(date);
    if (isNaN(d.getTime())) {
      throw new Error('Ngày không hợp lệ');
    }
    return d.toISOString();
  }

  /**
   * Định dạng ngày thành "YYYY-MM-DD" theo giờ địa phương, không chuyển sang UTC
   */
  formatToLocalDate(date: Date | string): string {
    const d = new Date(date);
    if (isNaN(d.getTime())) {
      throw new Error('Ngày không hợp lệ');
    }
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  /**
   * Định dạng ngày giờ thành "YYYY-MM-DD HH:mm:ss" theo giờ địa phương
   */
  formatToLocalDateTime(date: Date | string): string {
    const d = new Date(date);
    if (isNaN(d.getTime())) {
      throw new Error('Ngày không hợp lệ');
    }
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    const hours = String(d.getHours()).padStart(2, '0');
    const minutes = String(d.getMinutes()).padStart(2, '0');
    const seconds = String(d.getSeconds()).padStart(2, '0');
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  }
}