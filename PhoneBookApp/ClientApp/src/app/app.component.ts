import { Component, OnInit } from '@angular/core';
import { Contact } from './Interfaces/contact';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ApiserviceService } from './apiservice.service';
import { BehaviorSubject } from 'rxjs';




@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'Address-Book';
  showModal: boolean = false;
  contactForm!: FormGroup;
  updateContactForm!: FormGroup;
  numberDelete!: string;
  nameDelete!: string;
  deleteMessage: string = '';
  searchLastName: string = '';
  filteredContacts: Contact[] = [];
  deleteAppear!: boolean;
  cOK!: string;
  cFormAppear!: boolean;
  errStat!: boolean;
  errMessage!: string;
  succAppear!: boolean;
  contactId!: string;
  modelFirstName!: string;
  modelLastName!: string;
  modelPhoneNumber!: string;
  succUpdate!: boolean;
  errUpdate!: boolean;

  

  private contactsSubject = new BehaviorSubject<Contact[]>([]);
  contacts$ = this.contactsSubject.asObservable();
  

  constructor(private fb: FormBuilder,
              private api: ApiserviceService,
             ) {
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(10)]]
    });
  }




  
  deleteConf(phoneNumber:string, fistName:string, lastName:string){
      this.numberDelete = phoneNumber;
      this.nameDelete = `${fistName} ${lastName}`;
      this.deleteMessage = `Do you want to delete ${this.nameDelete} from contacts?`;
      this.deleteAppear = true;
      this.cOK = 'Cancel';
  }
 

  deleteContact(contactId: string ): void {
    this.api.delete(contactId).subscribe({
      next: res => {
        console.log('Deleted Successfully');
        this.deleteMessage = "Sucessfully Deleted"
        this.deleteAppear = false;
        this.cOK = 'OK';
        this.refreshContacts();
      },
      error: err => console.log(err),
      complete: () => console.log('Delete Call Complete')
    })
  }

  ngOnInit(): void {
    this.refreshContacts();
  }


  private refreshContacts(): void {
    this.api.getPhoneBook().subscribe({
      next: (res: Contact[]) => {
        console.log(res);
        this.contactsSubject.next(res);
        this.updateFilteredContacts();
      },
      error: err => {
        console.error(err);
      },
      complete() {
        console.log('PhoneBook call completed');
      },
    });
  }

  formApp(){
    this.cFormAppear = true;
    this.contactForm.reset();
  }

  addContact(){
    this.api.postPhoneBook(this.contactForm.value).subscribe({
      next: res => {
        console.log(res);
        this.refreshContacts();
        this.contactForm.reset();
        this.cFormAppear = false;
        this.succAppear = true;

      },
      error: err => {
        if(err.status == 409){
           this.cFormAppear = false;
           this.errStat = true;
           this.errMessage = 'Phone Number Already Exists. Please Make Changes Or Exit';
        }
        console.log(err.status);
      },
      complete: () => {
        console.log('Post call complete');
        
      }
    })
  }

  updateFilteredContacts(): void {
    const allContacts = this.contactsSubject.value;
    this.filteredContacts = this.searchLastName
      ? allContacts.filter((contact) =>
          contact.lastName.toLowerCase().includes(this.searchLastName.toLowerCase())
        )
      : allContacts;
  }
  
  succDisappear(){
   this.succAppear = false;
  }

  makeChanges(): void {
    this.cFormAppear = true;
    this.errStat = false
  }

  sendId(contact: Contact): void{
      this.contactId = contact.bookId;
      this.modelFirstName = contact.firstName;
      this.modelLastName = contact.lastName;
      this.modelPhoneNumber = contact.phoneNumber;

      console.log(contact.bookId);
      console.log(contact.lastName);
      
      

      
  }

  updateForm(){

   const form  = {
         firstName : this.modelFirstName,
         lastName : this.modelLastName,
         phoneNumber : this.modelPhoneNumber
   }

    this.api.updateContact(this.contactId, form).subscribe({
      next: res => {
          console.log(res);
          this.refreshContacts();
          window.alert('Update Succeeded')
          
      },
      error: err => {
        if(err.status == 409){
          window.alert('No Changes Made')
        }
        console.log(this.updateContactForm.value);
      },
    })
  }
}
