import { Component, inject, OnInit } from '@angular/core';
import { HttpService } from '../../services/http';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { LucideAngularModule, Pencil, Trash2, LoaderCircle, X, Search } from 'lucide-angular';
import { IDepartment } from '../../types/IDepartment';
import { IEmployee } from '../../types/IEmployee';

@Component({
  selector: 'app-employee',
  imports: [LucideAngularModule, ReactiveFormsModule, FormsModule],
  templateUrl: './employee.html',
  styleUrl: './employee.css'
})
export class Employee implements OnInit {
  readonly Pencil = Pencil;
  readonly Trash2 = Trash2;
  readonly LoaderCircle = LoaderCircle;
  readonly X = X;
  readonly Search = Search;

  httpService = inject(HttpService);
  fb = inject(FormBuilder);
  
  departments: IDepartment[] = [];
  employeeForm!: FormGroup;
  
  employees: IEmployee[] = [];
  filteredEmployees: IEmployee[] = []; // ✅ Filtered list for display
  searchQuery: string = ''; // ✅ Search input value
  
  loading: boolean = true;
  isModalOpen: boolean = false;
  editId: number = 0;

  openModal() {
    this.isModalOpen = true;
    localStorage.setItem("modalOpen", "true");
  }

  closeModal() {
    this.isModalOpen = false;
    localStorage.setItem("modalOpen", "false");
    this.employeeForm.reset({
      id: 0,   
    });
    this.editId = 0;
    this.employeeForm.get('gender')?.enable();
    this.employeeForm.get('joiningDate')?.enable();
    this.employeeForm.get('dateOfBirth')?.enable();
  }

  getEmployees() {
    this.httpService.getEmployees().subscribe({
      next: (result) => {
        this.employees = result;
        this.filteredEmployees = result; // ✅ Initially show all
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  // ✅ Real-time search filter
  onSearchChange() {
    const query = this.searchQuery.toLowerCase().trim();
    
    if (!query) {
      this.filteredEmployees = this.employees; // Show all if empty
      return;
    }

    this.filteredEmployees = this.employees.filter(employee => 
      employee.name.toLowerCase().includes(query) ||
      employee.email.toLowerCase().includes(query) ||
      employee.phone.toLowerCase().includes(query) ||
      employee.id.toString().includes(query)
    );
  }

  // ✅ Clear search
  clearSearch() {
    this.searchQuery = '';
    this.filteredEmployees = this.employees;
  }

  addEmployee() {
    console.log('New Employee:', this.employeeForm.value);
    this.httpService.addEmployee(this.employeeForm.value).subscribe({
      next: () => {
        this.getEmployees();
        alert('Employee added successfully');
      },
      error: () => {
        alert('Failed to add employee');
      }
    });
    this.closeModal();
  }

  editEmployee(employee: IEmployee) {
    let value = employee.id;
    this.httpService.getEmployeeById(value).subscribe((result) => {
      console.log(result);
      this.openModal();
      this.employeeForm.patchValue(result as any);
      this.employeeForm.get('gender')?.disable();
      this.employeeForm.get('joiningDate')?.disable();
      this.employeeForm.get('dateOfBirth')?.disable();
      this.editId = employee.id;
    });
  }

  updateEmployee() {
    if (this.employeeForm.invalid) return;

    const employeeData = this.employeeForm.value;

    this.httpService.updateEmployee(this.editId, employeeData).subscribe({
      next: () => {
        this.getEmployees();
        alert('Employee updated successfully');
        this.closeModal();
        this.editId = 0;
      },
      error: () => {
        alert('Failed to update employee');
      }
    });
  }

  getDepartments() {
    this.httpService.getDepartments().subscribe({
      next: (result) => {
        this.departments = result;
      },
      error: () => {
        alert('Failed to fetch departments');
      }
    });
  }

  deleteEmployee(id: number) {
    if (!confirm('Are you sure you want to delete this employee?')) return;
    
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

  ngOnInit() {
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
      lastWorkingDate: [''],
      dateOfBirth: ['', Validators.required],
    });
  }
}