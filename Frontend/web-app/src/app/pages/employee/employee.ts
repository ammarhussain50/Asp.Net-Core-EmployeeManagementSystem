import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { HttpService } from '../../services/http';
// import { LucideAngularModule, Plus, Edit, Trash2, X, LoaderCircle } from 'lucide-angular';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-employee',
  imports: [FormsModule,DatePipe],
  templateUrl: './employee.html',
  styleUrl: './employee.css',
})
export class Employee {
  httpservice = inject(HttpService);

  employees: any[] = [];
  isLoading: boolean = false;

  // Modal states
  showModal: boolean = false;
  editId: number = 0;

  // Form object
  employeeObj: any = {
    id: 0,
    name: '',
    email: '',
    phone: '',
    jobTitle: '',
    gender: 1,
    departmentId: null,
    dateOfBirth: '',
    joiningDate: '',
    lastWorkingDate: '',
  };

  ngOnInit() {
    this.getEmployees();
  }

  getEmployees() {
    // this.isLoading = true;
    this.httpservice.getEmployeeList().subscribe((result: any) => {
      this.employees = result;
      // this.isLoading = false;
    });
  }

  openModal() {
    this.resetForm();
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  resetForm() {
    this.employeeObj = {
      id: 0,
      name: '',
      email: '',
      phone: '',
      jobTitle: '',
      gender: 1,
      departmentId: null,
      dateOfBirth: '',
      joiningDate: '',
      lastWorkingDate: '',
    };
    this.editId = 0;
  }

  // Placeholder functions (tum khud implement karoge)
  saveEmployee() {
    console.log('Save function baad me implement hoga:', this.employeeObj);
  }

  editEmployee(item: any) {
    console.log('Edit function baad me implement hoga:', item);
  }

  updateEmployee() {
    console.log('Update function baad me implement hoga:', this.employeeObj);
  }

  deleteEmployee(id: number) {
    console.log('Delete function baad me implement hoga. ID:', id);
  }
}
