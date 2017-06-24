import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';
 
@inject(HttpClient)
export class Api {

    private http: HttpClient = null;
    public heading: string = 'Spells';
    public data: string = '';
    public code: string = '';
    private loading: boolean = false;
  isRequesting = false;

    constructor() {
            this.http = new HttpClient().configure(x => {
                x.withBaseUrl("http://localhost:49435");
            });
    }

    getPlayersList(){    
        return this.http.createRequest("/players")
        .asGet()
        .send()
        .then(resp => JSON.parse(resp.response));
    }


    getPlayerDetails(id){
        return this.http.createRequest("/player/" + id)
        .asGet()
        .send()
        .then(resp => JSON.parse(resp.response));
    }  

      savePlayer(player){
        return this.http.createRequest("/player/")
        .asPut()
        .withContent(player)
        .send()
        .then(resp => JSON.parse(resp.response));        
  }
}