import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})

export class DateFormatterService {
    /**
     * Định dạng ngày giờ thành chuỗi ISO 8601 với định dạng "YYYY-MM-DDTHH:mm:ss"
     * @param date Ngày cần định dạng (Date hoặc string)
     * @returns Chuỗi ngày giờ theo định dạng "YYYY-MM-DDTHH:mm:ss"
     */
    formatToISOString(date: Date | string): string {
      const d = new Date(date);
      if (isNaN(d.getTime())) {
        throw new Error('Ngày không hợp lệ');
      }
      return d.toISOString();
    }
}