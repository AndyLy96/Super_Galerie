import { PhotoImage } from "./PhotoImage";

export class Gallerie {
    constructor(public id : number, public name : string, public isPublic : boolean, public photoImages : PhotoImage[], public fileName : string){}
}
