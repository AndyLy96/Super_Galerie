import { ServiceService } from './../Tp3Service/service.service';
import { Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Gallerie } from '../models/Gallerie';

declare var Masonry : any;
declare var imagesLoaded : any;

@Component({
  selector: 'app-publicGalleries',
  templateUrl: './publicGalleries.component.html',
  styleUrls: ['./publicGalleries.component.css']
})
export class PublicGalleriesComponent implements OnInit {

  @ViewChild('masongrid') masongrid?: ElementRef; 
  @ViewChildren('masongriditems') masongriditems?: QueryList<any>; 

  constructor(public service : ServiceService) { }

  selectedGal? : Gallerie;
  truefalse : boolean = false;
  sizelink : string = "https://localhost:7171/api/Pictures/GetPhoto/miniature/";
  newLink : string = "https://localhost:7171/api/Pictures/GetPhoto/original/";
  photos : number[] = [];

  ngOnInit() {
    this.service.GetPublicGalleries();
  }

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

}


