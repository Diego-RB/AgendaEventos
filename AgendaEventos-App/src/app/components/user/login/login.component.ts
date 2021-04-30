import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  titulo = 'Login';
  model: any = {};
  jwtHelper = new JwtHelperService();
  chave: boolean;


  constructor(private authService: AuthService
            , public router: Router
            , private toastr: ToastrService) { }

  ngOnInit() {
    const token = localStorage.getItem('token');
    this.chave = this.jwtHelper.isTokenExpired(token);
    if (localStorage.getItem('token') != null && this.chave === false) {
      this.router.navigate(['/dashboard']);
    }else{
      localStorage.removeItem('token');
      this.router.navigate(['/user/login']);
    }
  }

  login() {
    this.authService.login(this.model)
      .subscribe(
        () => {
          this.router.navigate(['/dashboard']);
          this.toastr.success('Logado com Sucesso');
        },
        error => {
          this.toastr.error('Falha ao tentar Logar');
        }
      );
  }

}
