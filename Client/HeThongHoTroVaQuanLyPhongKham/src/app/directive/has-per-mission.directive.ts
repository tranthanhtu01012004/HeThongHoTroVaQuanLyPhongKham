import { Directive, Input, ElementRef, Renderer2 } from '@angular/core';
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
    this.permissions = Array.isArray(permissions) ? permissions : [permissions];
    this.updateView();
  }

  private updateView(): void {
    const hasPermission = this.permissionService.hasAnyRole(this.permissions);
    const button = this.el.nativeElement.tagName === 'BUTTON' 
      ? this.el.nativeElement 
      : this.el.nativeElement.querySelector('button');

    if (hasPermission) {
      this.renderer.removeAttribute(button, 'disabled');
      this.renderer.removeClass(button, 'locked');
    } else {
      this.renderer.setAttribute(button, 'disabled', 'true');
      this.renderer.addClass(button, 'locked');
    }
  }
}