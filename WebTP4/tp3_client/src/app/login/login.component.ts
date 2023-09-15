import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../Tp3Service/service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public router : Router, public service : ServiceService) { }

  username : string = "";
  passw : string = "";

  ngOnInit() {
  }

  async login(){
    // Retourner à la page d'accueil
    if(this.username != "" || this.passw != "")
    {
      await this.service.login(this.username,this.passw);

      this.router.navigate(['/publicGalleries']);
    }
   
  }

}
