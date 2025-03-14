import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [],
  templateUrl: './unauthorized.component.html',
  styleUrl: './unauthorized.component.css'
})
export class UnauthorizedComponent {
  returnUrl: string | null = null;

  constructor(private route: ActivatedRoute, private router: Router) {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  goToHome(): void {
    this.router.navigate(['/']);
  }
}