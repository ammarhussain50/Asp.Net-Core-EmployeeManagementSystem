import { Component, inject, OnInit } from '@angular/core';
import { LucideAngularModule,  Trash2, LoaderCircle,  } from 'lucide-angular';
import { Leaves } from '../../services/leave';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-leave',
  imports: [LucideAngularModule , DatePipe],
  templateUrl: './leave.html',
  styleUrl: './leave.css'
})
export class Leave   {
  Math = Math; // ✅ expose Math to your template
  readonly Trash2 = Trash2;
  readonly LoaderCircle = LoaderCircle;
  

  leaveService = inject(Leaves);
 

  leaves: any[] = [];
  loading: boolean = true;

  // pagination
  filter: any = {};
  pageIndex: number = 0;
  pageSize: number = 5;
  totalCount: number = 0;

  
  editId = 0;



  // initialize
  ngOnInit(): void {
    this.getLeaves();

  }

  // fetch all leaves
  getLeaves() {
    this.filter.pageIndex = this.pageIndex;
    this.filter.pageSize = this.pageSize;

    this.loading = true;
    this.leaveService.getLeave(this.filter).subscribe({
      next: (result : any) => {
        this.leaves = result.data || result; // adjust if API returns plain list
        this.totalCount = result.totalCount || result.length;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        alert('Failed to fetch leaves');
      }
    });
  }

  onPageChange(newPage: number) {
  this.pageIndex = newPage;
  this.getLeaves();
}


 


}
