import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from './Tp3Service/service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public router : Router, public service : ServiceService) { }


  logout()
  {
    this.service.logout();
    this.router.navigate(['/login']);
  }

}
