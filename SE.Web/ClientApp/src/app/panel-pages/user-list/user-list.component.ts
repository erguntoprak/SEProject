import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { BlogListModel, UserListModel } from '../../shared/models';
import Swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';
import * as _ from 'lodash';
import { AcdcLoadingService } from 'acdc-loading';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { UserRoleEditComponent } from '../user-role-edit/user-role-edit.component';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'se-user-list',
  templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit {

  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'phoneNumber', 'roleName', 'emailConfirmed', 'isActive', 'actions'];
  dataSource;

  constructor(private baseService: BaseService, private acdcLoadingService: AcdcLoadingService, private toastr: ToastrService) {

  }
  ngOnInit(): void {
    this.getAllUserList();
  }
  applyFilter(filterValue: string) {
    filterValue = filterValue.trim();
    filterValue = filterValue.toLowerCase();
    this.dataSource.filter = filterValue;
  }

  getAllUserList() {
    this.acdcLoadingService.showLoading();
    this.baseService.getAll<UserListModel[]>("Account/GetAllUserList").subscribe(userList => {
      this.dataSource = new MatTableDataSource(userList);
      this.dataSource.sort = this.sort;
      this.acdcLoadingService.hideLoading();
    });
  }
  
  onDeleteUser(userId: string) {
    Swal.fire({
      title: 'Kullanıcı Silme',
      text: "Kullanıcı silinecek emin misiniz ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#6754e2',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText: 'Hayır',
      focusCancel: true
    }).then((result) => {
      if (result.value) {

        this.baseService.post("Account/DeleteUser",
          userId).subscribe(data => {
            this.toastr.success('Kullanıcı Silindi.', 'Başarılı!');
            this.getAllUserList();
            this.acdcLoadingService.hideLoading();
          },
            (error: HttpErrorResponse) => {
              this.toastr.success(error.error, 'Başarısız!');
              this.acdcLoadingService.hideLoading();
            });
      }
    })
  }
  onEmailConfirm(userId: string, emailConfirmation: boolean) {
    Swal.fire({
      title: emailConfirmation ? 'E-posta Onayla' : 'E-posta Onayını Geri Al',
      text: emailConfirmation ? "Kullanıcı e-posta onayı verilecek emin misiniz ?" :"Kullanıcı e-posta onayı geri alınacak emin misiniz ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#6754e2',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, onayla!',
      cancelButtonText: 'Hayır',
      focusCancel: true
    }).then((result) => {
      if (result.value) {

        this.baseService.post("Account/UpdateEmailConfirmation",
          { userId: userId, emailConfirmation: emailConfirmation}).subscribe(data => {
            this.toastr.success(emailConfirmation ? 'E-posta Onaylandı.':'E-posta onayı geri alındı.', 'Başarılı!');
            this.getAllUserList();
            this.acdcLoadingService.hideLoading();
          },
            (error: HttpErrorResponse) => {
              this.toastr.success(error.error, 'Başarısız!');
              this.acdcLoadingService.hideLoading();
            });
      }
    })
  }
  onActivateUser(userId: string, isActive: boolean) {
    Swal.fire({
      title: isActive ? 'Kullanıcı Aktif Et' : 'Kullanıcı Aktif Durumunu Geri Al',
      text: isActive ? "Kullanıcı aktif edilecek emin misiniz ?" : "Kullanıcı aktif durumu geri alınacak emin misiniz ?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#6754e2',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, onayla!',
      cancelButtonText: 'Hayır',
      focusCancel: true
    }).then((result) => {
      if (result.value) {

        this.baseService.post("Account/UpdateUserActivate",
          { userId: userId, isActive: isActive }).subscribe(data => {
            this.toastr.success(isActive ? 'Kullanıcı Aktif Edildi.' : 'Kullanıcı Aktif Durumu Geri Alındı.', 'Başarılı!');
            this.getAllUserList();
            this.acdcLoadingService.hideLoading();
          },
            (error: HttpErrorResponse) => {
              this.toastr.success(error.error, 'Başarısız!');
              this.acdcLoadingService.hideLoading();
            });
      }
    })
  }


}

