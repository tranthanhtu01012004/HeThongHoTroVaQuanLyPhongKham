import { Injectable } from '@angular/core';
import { NativeDateAdapter } from '@angular/material/core';

// Định dạng ngày tùy chỉnh cho mat-datepicker
export const CUSTOM_DATE_FORMATS = {
    parse: {
      dateInput: 'DD/MM/YYYY', // Định dạng khi nhập tay
    },
    display: {
      dateInput: 'DD/MM/YYYY', // Định dạng hiển thị
      monthYearLabel: 'MMM YYYY',
      dateA11yLabel: 'LL',
      monthYearA11yLabel: 'MMMM YYYY',
    },
  };
  
  // Custom DateAdapter để hỗ trợ nhập tay dd/MM/yyyy
  @Injectable()
  export class CustomDateAdapter extends NativeDateAdapter {
    override parse(value: any): Date | null {
      if (typeof value === 'string' && value.includes('/')) {
        const [day, month, year] = value.split('/').map(Number);
        return new Date(year, month - 1, day); // Ép buộc DD/MM/YYYY
      }
      return super.parse(value);
    }
  
    override format(date: Date, displayFormat: any): string {
      if (displayFormat === 'DD/MM/YYYY') {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}/${month}/${year}`; // Đảm bảo hiển thị DD/MM/YYYY
      }
      return super.format(date, displayFormat);
    }
  }