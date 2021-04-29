import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseURL = 'http://localhost:5000/api/user/';
  // jwtHelper = new JwtHelperService();
  decodedToken: any;


  constructor(private http: HttpClient, public router: Router) { }

  login(model: any) {
    return this.http
      .post(`${this.baseURL}login`, model).pipe(
        map((response: any) => {
          const user = response;

          if (user) {
            localStorage.setItem('token', user.token);
            // this.decodedToken = this.jwtHelper.decodeToken(user.token);
            sessionStorage.setItem('username', this.decodedToken.unique_name);
            sessionStorage.setItem('userid', this.decodedToken.nameid);
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(`${this.baseURL}register`, model);
  }

  loggedIn() {
      const token = localStorage.getItem('token');
     return !this.jwtHelper.isTokenExpired(token);

  }
}
