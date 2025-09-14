import { Component, inject } from '@angular/core';
import { HttpService } from '../../services/http';
import { IDepartment } from '../../types/IDepartment';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LucideAngularModule, Plus, Edit, Trash2, X, LoaderCircle } from 'lucide-angular';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FormsModule, LucideAngularModule],
  templateUrl: './departments.html',
  styleUrl: './departments.css',
})
export class Departments {
  httpService = inject(HttpService);
  departments: IDepartment[] = [];
  showModal: boolean = false;
  newDepartment: string = '';
  editId: number = 0;
  isLoading: boolean = false;

  // expose icons for template
  Plus = Plus;
  Edit = Edit;
  Trash2 = Trash2;
  X = X;
  LoaderCircle = LoaderCircle;

  ngOnInit() {
    this.getLatestData();
  }

 getLatestData() {
  this.isLoading = true;
  this.httpService.getDepartments().subscribe({
    next: (result) => {
      
        this.departments = result;
        this.isLoading = false;
    },
    error: () => {
      this.isLoading = false;
    }
  });
}


  openModal() {
    this.showModal = true;
    this.newDepartment = '';
    this.editId = 0;
  }

  closeModal() {
    this.showModal = false;
    this.newDepartment = '';
    this.editId = 0;
  }

  saveDepartment() {
    this.isLoading = true;
    this.httpService.addDepartment(this.newDepartment).subscribe({
      next: () => {
        alert('Department added successfully');
        this.closeModal();
        this.getLatestData();
        this.isLoading = false;
      },
      error: () => (this.isLoading = false)
    });
  }

  editDepartment(department: IDepartment) {
    this.newDepartment = department.name;
    this.editId = department.id;
    this.showModal = true;
  }

  updateDepartment() {
    this.isLoading = true;
    this.httpService.updateDepartment(this.editId, this.newDepartment).subscribe({
      next: () => {
        alert('Department updated successfully');
        this.closeModal();
        this.getLatestData();
        this.editId = 0;
        this.isLoading = false;
      },
      error: () => (this.isLoading = false)
    });
  }

  deleteDepartment(id: number) {
    this.isLoading = true;
    this.httpService.deleteDepartment(id).subscribe({
      next: () => {
        alert('Department deleted successfully');
        this.getLatestData();
        this.isLoading = false;
      },
      error: () => (this.isLoading = false)
    });
  }
}
