import { Component, inject, OnInit } from '@angular/core';
import { LucideAngularModule, Users, Building2, Wallet } from 'lucide-angular';
import { ChartConfiguration } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { IDepartmentDashboard } from '../../types/IDashboard';
import { Dashboard } from '../../services/dashboard';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [LucideAngularModule, BaseChartDirective],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

  departmentData!: IDepartmentDashboard[];

  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Employee Count',
        backgroundColor: '#3b82f6', // blue-500
        borderRadius: 6,
      }
    ]
  };

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false
        }
      },
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 20
        }
      }
    },
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          color: '#374151', // gray-700
          font: {
            size: 13
          }
        }
      }
    }
  };

  totalSalary!: number;
  employeeCount!: number;
  departmentCount!: number;

  // icons
  readonly Users = Users;
  readonly Building2 = Building2;
  readonly Wallet = Wallet;

  dashboardService = inject(Dashboard);

  ngOnInit(): void {
    this.getDashboardData();
    this.getDepartmentData();
  }

  getDashboardData() {
    this.dashboardService.getDashboardData().subscribe({
      next: (result) => {
        this.totalSalary = result.totalSalary;
        this.employeeCount = result.employeeCount;
        this.departmentCount = result.departmentCount;
      },
      error: () => {
        alert('Failed to fetch dashboard data');
      }
    });
  }


   getDepartmentData() {
    this.dashboardService.getDepartmentData().subscribe({
      next: (result) => {

        this.barChartData.labels = result.map(dept => dept.name);
        this.barChartData.datasets[0].data = result.map(dept => dept.employeeCount);
        this.departmentData = result;
      },
      error: () => {
        alert('Failed to fetch department data');
      }
    });
  }
}