import { Routes } from '@angular/router';
import { TrangChuComponent } from './users/components/trang-chu/trang-chu.component';
import { GioiThieuComponent } from './users/components/gioi-thieu/gioi-thieu.component';
import { DichVuComponent } from './users/components/dich-vu/dich-vu.component';
import { LichHenComponent } from './users/components/lich-hen/lich-hen.component';
import { DangNhapComponent } from './users/components/dang-nhap/dang-nhap.component';
import { UserLayoutComponent } from './users/components/user-layout.component';
import { AdminLayoutComponent } from './admins/components/admin-layout/admin-layout.component';
import { PhongKhamComponent } from './admins/components/phong-kham/phong-kham.component';
import { DichVuYTeComponent } from './admins/components/dich-vu-y-te/dich-vu-y-te.component';
import { adminAuthGuard } from './guards/adminAuthGuard';
import { UnauthorizedComponent } from './users/components/unauthorized/unauthorized.component';
import { authGuard } from './guards/authGuards';
import { NhanVienComponent } from './admins/components/nhan-vien/nhan-vien.component';
import { QuanLyLichHenComponent } from './admins/components/quan-ly-lich-hen/quan-ly-lich-hen.component';
import { ThuocComponent } from './admins/components/thuoc/thuoc.component';
import { FilterAppointmentsComponent } from './admins/components/appointment-filter/appointment-filter.component';
import { HoSoYTeComponent } from './admins/components/ho-so-y-te/ho-so-y-te.component';
import { HoaDonComponent } from './admins/components/hoa-don/hoa-don.component';
import { BaoCaoComponent } from './admins/components/bao-cao/bao-cao.component';
import { UserChatComponent } from './users/components/user-chat/user-chat.component';
import { AdminChatComponent } from './admins/components/admin-chat/admin-chat.component';

export const routes: Routes = [
  { 
    path: '',
    component: UserLayoutComponent,
    children: [
      { path: '', component: TrangChuComponent },
      { path: 'gioi-thieu', component: GioiThieuComponent },
      { path: 'dich-vu', component: DichVuComponent },
      { path: 'lich-hen', component: LichHenComponent, canActivate: [authGuard] },
      { path: 'dang-nhap', component: DangNhapComponent },
      { path: 'unauthorized', component: UnauthorizedComponent },
      { path: 'chat', component: UserChatComponent, canActivate: [authGuard] }
    ]
  },

  //Admins
  { 
    path: 'admin', 
    component: AdminLayoutComponent,
    canActivate: [adminAuthGuard],
    children: [
      { path: 'bao-cao', component: BaoCaoComponent },
      { path: 'phong-kham', component: PhongKhamComponent },
      { path: 'dich-vu-y-te', component: DichVuYTeComponent },
      { path: 'nhan-vien', component: NhanVienComponent },
      { path: 'lich-hen', component: QuanLyLichHenComponent },
      { path: 'thuoc', component: ThuocComponent },
      { path: 'loc-lich-hen', component: FilterAppointmentsComponent },
      { path: 'ho-so-y-te', component: HoSoYTeComponent },
      { path: 'hoa-don', component: HoaDonComponent },
      { path: 'chat', component: AdminChatComponent }

    ]
  }
];
