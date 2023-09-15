import { Gallerie } from './../models/Gallerie';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { ElementRef, Injectable } from '@angular/core';
import { last, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  galeries : Gallerie[] = [];

constructor(public http : HttpClient) 
{

}

  async register(registerUsername : string, registerEmail : string, registerPassword : string, registerPasswordConfirm : string): Promise<void> {

    let registerDTO = {
      username :registerUsername,
      email : registerEmail,
      password : registerPassword,
      confirmPassword : registerPasswordConfirm};

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7171/" + "api/User/Register", registerDTO)); 
    console.log(x);

  }

  async login(loginUsername : string, loginPassword : string) : Promise<void>{
    let loginDTO = {
      username : loginUsername,
      password : loginPassword
    }

    let x = await lastValueFrom(this.http.post<any>("https://localhost:7171/" + "api/User/Login", loginDTO));
    console.log(x);
    localStorage.setItem("token", x.token);

  }

  async logout(){
    localStorage.removeItem("token");
  }

  async GetMyGalleries() : Promise<void>{
    let x = await lastValueFrom(this.http.get<Gallerie[]>("https://localhost:7171/" + "api/Galeries/MyGetGaleries"))
    this.galeries = x;
    console.log(x);
  }

  async GetPublicGalleries() : Promise<void>{
    let x = await lastValueFrom(this.http.get<Gallerie[]>("https://localhost:7171/" + "api/Galeries/PublicGetGalerie"))
    this.galeries = x;
  }

  async PostGalleries(name : string, isPublic : boolean, galPicInpute ?: ElementRef ): Promise<void>{
    let a = new Gallerie(0, name, isPublic, [], "");
    let x = await lastValueFrom(this.http.post<Gallerie>("https://localhost:7171/" + "api/Galeries/PostGalerie", a));
    console.log(x);
    let file = galPicInpute?.nativeElement.files[0];
    if(file == null){
      console.log("Input HTML ne contient aucune image");
      return;
    }
    else
    {
      let formData = new FormData();
      formData.append("monImage", file, file.name);
      let y = await lastValueFrom(this.http.post<any>("https://localhost:7171/api/Pictures/PostGalPhoto/" + x.id , formData ));
      console.log(y); 
    }
    this.GetMyGalleries();
  }

  async deleteGalerie(id : number) : Promise<void>{
    if(id != undefined){
      let x = await lastValueFrom(this.http.delete<Gallerie>("https://localhost:7171/" + "api/Galeries/DeleteGalerie/" + id));
      console.log(x);
    }
    this.GetMyGalleries();
  } 

  async PorPGalerie(id : number, galerie : Gallerie): Promise<void>{
    if(id != undefined){
      let x = await lastValueFrom(this.http.put<Gallerie>("https://localhost:7171/" + "api/Galeries/PorPGalerie/" + id, galerie));
      console.log(x);
    }
    this.GetMyGalleries();
  } 

  async ShareGalerie(id : number, username : string):Promise<void>{
    if(id != undefined){
      let x = await lastValueFrom(this.http.put<Gallerie>("https://localhost:7171/" + "api/Galeries/shareGalerie/" + id + "/" + username, null));
      console.log(x);
    }
    this.GetMyGalleries();
  } 

  async PostPhoto(pictureInpute : ElementRef, id:number) : Promise<void> {
    if(pictureInpute == undefined){
      console.log("Input HTML non chargé.");
      return;
    }
  
    let file = pictureInpute.nativeElement.files[0];
    if(file == null){
      console.log("Input HTML ne contient aucune image");
      return;
    }
    let formData = new FormData();
    formData.append("monImage", file, file.name);
  
    let x = await lastValueFrom(this.http.post<any>("https://localhost:7171/api/Pictures/PostPhoto/" + id, formData));
    console.log(x);
  }

  async DeleteBirb(id : number): Promise<void> {
    let x = await lastValueFrom(this.http.delete<any>("https://localhost:7171/api/Pictures/DeleteBirb/" + id));
    console.log(x);
  }

  async ChangerCouverture(galInpute : ElementRef,id : number): Promise<void> {
    if(galInpute == undefined){
      console.log("Input HTML non chargé.");
      return;
    }
  
    let file = galInpute.nativeElement.files[0];
    if(file == null){
      console.log("Input HTML ne contient aucune image");
      return;
    }
    let formData = new FormData();
    formData.append("monImage", file, file.name);
    let x = await lastValueFrom(this.http.post<any>("https://localhost:7171/api/Pictures/PostGalPhoto/" + id, formData ));
    console.log(x);
  }
 


}


