import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../Tp3Service/service.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(public router : Router, public service : ServiceService) { }

  username : string = "";
  email : string = "";
  passw : string = "";
  confirmP : string = "";

  ngOnInit() {

  }

  async register(){
    // Aller vers la page de connexion
    if(this.username != "" || this.email != "" || this.passw != "" || this.confirmP != "")
    {
      await this.service.register(this.username, this.email, this.passw, this.confirmP);

      this.router.navigate(['/login']);
    }
   
  }

}
