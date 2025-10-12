import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Departments } from './pages/departments/departments';
import { Employee } from './pages/employee/employee';
import { Login } from './pages/login/login';
import { EmployeeDashboard } from './pages/employee-dashboard/employee-dashboard';
import { Profile } from './pages/profile/profile';
import { Leave } from './pages/leave/leave';

export const routes: Routes = [

    {
        path:'departments',
        component:Departments
        
    },
    {
      path: '',
      component:Home
  },
    { 
        path: 'employee-dashboard', component: EmployeeDashboard,
       },
    {
        path : 'employee',
        component: Employee 
    },
    {
        path : 'login',
        component : Login
    },
    {
        path: 'Profile',
        component : Profile
    },
    {
        path : "Leave",
        component : Leave
    }
];
