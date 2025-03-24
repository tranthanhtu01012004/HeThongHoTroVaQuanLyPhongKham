import { Directive, Input, ElementRef, Renderer2, HostListener } from '@angular/core';
import { PermissionService } from '../services/permission/permission.service';

@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective {
  private permissions: string[] = [];

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private permissionService: PermissionService
  ) {}

  @Input() set hasPermission(permissions: string | string[]) {
    // Nếu là chuỗi, tách bằng dấu phẩy; nếu là mảng thì giữ nguyên
    this.permissions = typeof permissions === 'string' ? permissions.split(',').map(p => p.trim()) : permissions;
    this.updateView();
  }

  private updateView(): void {
    const hasPermission = this.permissionService.hasAnyRole(this.permissions);
    const element = this.el.nativeElement;

    if (hasPermission) {
      // Mở khóa
      this.renderer.removeClass(element, 'locked');
      this.renderer.removeStyle(element, 'opacity');
      this.renderer.removeStyle(element, 'cursor');
      if (element.tagName === 'BUTTON' || element.tagName === 'SELECT') {
        this.renderer.removeAttribute(element, 'disabled');
      }
    } else {
      // Khóa
      this.renderer.addClass(element, 'locked');
      this.renderer.setStyle(element, 'opacity', '0.6');
      this.renderer.setStyle(element, 'cursor', 'not-allowed');
      if (element.tagName === 'BUTTON' || element.tagName === 'SELECT') {
        this.renderer.setAttribute(element, 'disabled', 'true');
      }
    }
  }

  @HostListener('click', ['$event'])
  onClick(event: Event): void {
    if (!this.permissionService.hasAnyRole(this.permissions) && this.el.nativeElement.tagName === 'A') {
      event.preventDefault(); // Ngăn điều hướng cho <a>
    }
  }
}