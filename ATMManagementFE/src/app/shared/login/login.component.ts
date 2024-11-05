import {Component} from '@angular/core';
import {LoginService} from './login.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = "";
  password: string = "";
  errorMessage: string = "";

  constructor(private loginService: LoginService, private router: Router) {
  }

  onLogin() {
    this.loginService.login(this.email, this.password).then(
      response => {
        console.log("Login successful");
        if (response.token) {
          console.log("Token: ", response.token);
          this.errorMessage = "";
          localStorage.setItem('authToken', response.token);
        }
        this.router.navigate(['/home']);
      },
      error => {
        console.log("Login failed");
        this.errorMessage = "Login failed!";
        console.log("Error to login: ", error);
      }
    );
  }
}
