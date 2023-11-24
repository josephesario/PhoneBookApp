import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiserviceService {


  private baseUrl : string = environment.apiUrl;
  constructor(private http: HttpClient) { }


  getPhoneBook(){

    return this.http.get<any>(`${this.baseUrl}/PhoneBook/GetAllPhoneBooks`);
  };


  postPhoneBook(contactData:FormGroup){


    
    return this.http.post(`${this.baseUrl}/PhoneBook/AddPhoneBook`,contactData, {responseType: 'text'});
  }


  delete(phoneNumber:string){
     return this.http.delete(`${this.baseUrl}/PhoneBook/DeleteByPhoneNumber/${phoneNumber}`,{responseType: 'text'});
  }

  updateContact(BookId: string, bookData: object){
    return this.http.put(`${this.baseUrl}/PhoneBook/UpdateContactById/${BookId}`, bookData,{responseType: 'text'});
  }

}
