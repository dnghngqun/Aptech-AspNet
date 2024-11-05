import {Injectable} from '@angular/core';
import axios from 'axios';
@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private URLAPI: string = 'http://localhost:5131/api/auth/login';

  constructor() {
  }

  login(email: string, password: string):Promise<any> {
    const body = {email, password};
    return axios.post(this.URLAPI,body)
    .then(response => response.data)
    .catch(err => {
      throw err;
    })
  }
}
