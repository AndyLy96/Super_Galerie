import { Gallerie } from './../models/Gallerie';
import { ServiceService } from './../Tp3Service/service.service';
import { Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';


declare var Masonry : any;
declare var imagesLoaded : any;

@Component({
  selector: 'app-myGalleries',
  templateUrl: './myGalleries.component.html',
  styleUrls: ['./myGalleries.component.css']
})
export class MyGalleriesComponent implements OnInit {
  
  @ViewChild('masongrid') masongrid?: ElementRef; 
  @ViewChildren('masongriditems') masongriditems?: QueryList<any>; 


  @ViewChild("myPictureViewChild", {static:false}) pictureInpute ?: ElementRef;
  @ViewChild("myGalViewChild", {static:false}) galInpute ?: ElementRef;
  @ViewChild("myGalPictureViewChild", {static:false}) galPicInpute ?: ElementRef;


  constructor(public service : ServiceService) { }
  
  imgCover : string = "/assets/images/galleryThumbnail.png";
  truefalse : boolean = false;
  sizelink : string = "https://localhost:7171/api/Pictures/GetPhoto/miniature/";
  newLink : string = "https://localhost:7171/api/Pictures/GetPhoto/original/";
  photos : number[] = [];
  name : string = "";
  username : string = "";
  isPublic : boolean = true;
  selectedGal? : Gallerie;
  idOffical:number | undefined;

  ngAfterViewInit() { 
        this.masongriditems?.changes.subscribe(e => { 
          this.initMasonry(); 
        }); 
      
        if(this.masongriditems!.length > 0) { 
          this.initMasonry(); 
        } 
      } 
    
      initMasonry() { 
        var grid = this.masongrid?.nativeElement; 
        var msnry = new Masonry( grid, { 
          itemSelector: '.grid-item',
          columnWidth:320, // À modifier si le résultat est moche
          gutter:3
        });
       
        imagesLoaded( grid ).on( 'progress', function() { 
          msnry.layout(); 
        }); 
      } 
    
fonction()
{
  this.service.PostGalleries(this.name, this.isPublic,this.galPicInpute);
}


couverture(id:number)
{
 
  if(this.galInpute != undefined && this.selectedGal != null)
  {
    this.service.ChangerCouverture(this.galInpute, this.selectedGal?.id)
    this.imgCover = "https://localhost:7171/api/Pictures/GetPhoto/original/" + id;

  }

}

delete(id : number)
{
  console.log(id);
  for(let gal of this.service.galeries)
  {
    if(gal.id == id)
    {
       this.selectedGal = gal;
    }
  }
}

changerSize(id : number)
{
  this.newLink = "https://localhost:7171/api/Pictures/GetPhoto/original/";
 this.truefalse = !this.truefalse;
  this.newLink = this.newLink + id;
 console.log(this.newLink)
}

changeValue(value : boolean)
{
  this.truefalse = value;
}

uploadPicture()
{
 if(this.pictureInpute != undefined && this.selectedGal != null)
 {
  this.service.PostPhoto(this.pictureInpute, this.selectedGal?.id)
 }

  
}


deletePhoto(id : number)
{
  this.service.DeleteBirb(id)
}

  ngOnInit() {
    this.service.GetMyGalleries();
    
    }



}


