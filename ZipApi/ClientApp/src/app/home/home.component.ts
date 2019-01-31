import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public locations: Location[];
  public http: HttpClient;
    public baseUrl: string;
    public zipcode: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
   
  }

  public getLocationData(zip: string) {
    this.http.get<Location[]>(this.baseUrl + 'api/locations/' + zip).subscribe(result => {
      this.locations = result;
    }, error => console.error(error));
  }

  public loadData() {
    this.http.post(this.baseUrl + 'api/locations/action/load-data', null).subscribe(result => {  
    }, error => console.error(error));
  }
  
}

interface Location {
  zip: string;
  cbsa: string;
  msa: string;
  pop2015: string;
  pop2014: string;
}