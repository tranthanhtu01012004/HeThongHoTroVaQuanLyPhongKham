import { Routes } from '@angular/router';
import { TrangChuComponent } from './users/components/trang-chu/trang-chu.component';
import { GioiThieuComponent } from './users/components/gioi-thieu/gioi-thieu.component';
import { DichVuComponent } from './users/components/dich-vu/dich-vu.component';
import { LichHenComponent } from './users/components/lich-hen/lich-hen.component';
import { DangNhapComponent } from './users/components/dang-nhap/dang-nhap.component';
import { UserLayoutComponent } from './users/components/user-layout.component';

export const routes: Routes = [
  { 
    path: '',
    component: UserLayoutComponent,
    children: [
      { path: '', component: TrangChuComponent },
      { path: 'gioi-thieu', component: GioiThieuComponent },
      { path: 'dich-vu', component: DichVuComponent },
      { path: 'lich-hen', component: LichHenComponent },
      { path: 'dang-nhap', component: DangNhapComponent }
    ]
  },

  //Admins
  // { 
  //   path: 'admin', 
  //   component: AdminLayoutComponent,
  //   // canActivate: [authRoleGuard],
  //   children: [
  //     { path: 'phong-kham', component: PhongkhamComponent }
  //   ]
  // }
];
