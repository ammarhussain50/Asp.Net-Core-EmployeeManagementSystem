import { Component, inject, OnInit } from '@angular/core';
import { HttpService } from '../../services/http';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { LucideAngularModule, Pencil, Trash2, LoaderCircle, X,Eye} from 'lucide-angular'; // Loader icon

import { IDepartment } from '../../types/IDepartment';
import { IEmployee } from '../../types/IEmployee';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employee',
  imports: [LucideAngularModule,ReactiveFormsModule],
  templateUrl: './employee.html',
  styleUrl: './employee.css'
})
export class Employee implements OnInit {


  viewAttendance(employeeId: number) {

  this.router.navigateByUrl(`/attendance/${employeeId}`);
 
}


  router = inject(Router);
  Math = Math; // ✅ expose Math to your template
readonly Pencil = Pencil;
readonly Eye = Eye;
  readonly Trash2 = Trash2;
  readonly LoaderCircle = LoaderCircle;
  searchControl = new FormControl('');
filter: any = {

};   //  isme saare filters store honge
pageIndex : number  = 0;
pageSize : number  = 5;
totalCount : number  = 0;

  httpService = inject(HttpService);
  

  departments: IDepartment[] = [];
  fb = inject(FormBuilder);
    employeeForm!: FormGroup;

  employees: IEmployee[] = [];
  loading:boolean = true; // track loading
    // modal state
  isModalOpen: boolean = false;
  editId: number = 0;
  DepartmentName: string = '';
  readonly X = X;

    openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.employeeForm.reset({
       id: 0,   
    });
    this.editId = 0;
      this.employeeForm.get('gender')?.enable();
      this.employeeForm.get('joiningDate')?.enable();
      this.employeeForm.get('dateOfBirth')?.enable();
  }


getEmployees() {
    this.filter.pageIndex = this.pageIndex;
  this.filter.pageSize = this.pageSize;
    this.httpService.getEmployees(this.filter).subscribe({
      next: (result) => {
        this.employees = result.data;
        this.totalCount = result.totalCount;
        this.loading = false; // stop loader when data arrives
      },
      error: () => {
        this.loading = false; // stop loader on error too
      }
    });

}

  addEmployee() {
    console.log('New Employee:', this.employeeForm.value);
    this.httpService.addEmployee(this.employeeForm.value).subscribe({
      next: () => {
        this.getEmployees(); // Refresh the list after adding
        alert('Employee added successfully');
      },
      error: () => {
        alert('Failed to add employee');
      }
    });

    this.closeModal();
  }

  editEmployee(employee: IEmployee) {
    
  
    this.editId = employee.id;
    console.log(employee);
    
    this.employeeForm.patchValue(
      employee
    );
     this.employeeForm.get('gender')?.disable();
     this.employeeForm.get('joiningDate')?.disable();
     this.employeeForm.get('dateOfBirth')?.disable();


    this.openModal();
    
    

  }
  updateEmployee(){
    this.httpService.updateEmployee(this.editId,this.employeeForm.value).subscribe({
      next:()=>{
        this.getEmployees();
        alert('Employee updated successfully');
        this.closeModal();
        this.editId = 0;
      },
      error:()=>{
        alert('Failed to update employee');
      }
    });

  }
  getDepartments() {
    this.httpService.getDepartments({}).subscribe({
      next: (result) => {
        this.departments = result.data;
     
      },
      error: () => {
        alert('Failed to fetch departments');
    
      }
    });

}

  deleteEmployee(id: number) {
    this.httpService.deleteEmployee(id).subscribe({
      next: () => {
        this.getEmployees();
        alert('Employee deleted successfully');
      },
      error: () => {
        alert('Failed to delete employee');
      }
    });
  }
  // page change handler
// page change handler
onPageChange(newPage: number) {
  this.pageIndex = newPage;
  this.getEmployees();
}



  ngOnInit() : void {
     // jab search change ho
  this.searchControl.valueChanges.pipe(debounceTime(300), distinctUntilChanged()).subscribe((result: string | null) => {
    this.filter.search = result || '';   // agar null ho to empty string
     
    this.pageIndex = 0; // reset to first page on new search
    console.log(result);
    
    this.getEmployees();                 // call reload
  });

    this.getEmployees();
    this.getDepartments();

  this.employeeForm = this.fb.group({
    id: [0],
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,15}$/)]],
      gender: [null, Validators.required],
      jobTitle: [null, Validators.required],
      departmentId: [null, Validators.required],
      joiningDate: ['', Validators.required],
      lastWorkingDate: [''], // optional
      dateOfBirth: ['', Validators.required],
    });
  
  }

}