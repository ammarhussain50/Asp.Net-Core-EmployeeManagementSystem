import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Eye, EyeClosed, LoaderCircle, LucideAngularModule, Pencil } from 'lucide-angular';
import { Router } from '@angular/router';
import { HttpService } from '../../services/http';
import { IProfile, IProfileResponse } from '../../types/IProfile';

@Component({
  selector: 'app-profile',
  imports: [LucideAngularModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit {
  fb = inject(FormBuilder);
  httpService = inject(HttpService);
  authService = inject(Auth);
  router = inject(Router);

  profileForm!: FormGroup;
  loading = false;
  showOldPassword = false;
  showNewPassword = false;

  readonly LoaderCircle = LoaderCircle;
  readonly eye = Eye;
  readonly eyeClosed = EyeClosed;
   readonly Pencil = Pencil;

  ngOnInit(): void {
    this.initForm();
    this.getProfile();
  }

  initForm() {
    this.profileForm = this.fb.group({
      userId: [this.authService.AuthDetail?.id || ''],
      name: ['', [Validators.required, Validators.minLength(3)]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9]{10,15}$/)]],
      profileImage: [''],
      oldPassword: [''],
      newPassword: [''],
       salary :[]
    });
  }

  toggleOldPassword() {
    this.showOldPassword = !this.showOldPassword;
  }

  toggleNewPassword() {
    this.showNewPassword = !this.showNewPassword;
  }

  getProfile() {
    const userId = this.authService.AuthDetail?.id || '';
    console.log(userId)
    this.httpService.getProfile(userId).subscribe({
      next: (res: IProfileResponse) => {
        this.profileForm.patchValue({
          name: res.name,
          phone: res.phone,
          profileImage: res.profileImage || '',
          salary: res.salary || 0  // ✅ add this line
        });
      },
      error: () => {
        alert('Failed to load profile. Please try again.');
      }
    });
  }

  updateProfile() {
    if (this.profileForm.invalid) {
      alert('Please fill all required fields correctly');
      return;
    }

    this.loading = true;
   

    this.httpService.updateProfile(this.profileForm.value).subscribe({
      next: (res: any) => {
        this.loading = false;
        // Backend string return karta hai directly
        alert(typeof res === 'string' ? res : res.message || 'Profile updated successfully');
        
        // Password fields clear karo after successful update
        this.profileForm.patchValue({
          oldPassword: '',
          newPassword: ''
        });
        
        this.router.navigate(['/']);
      },
      error: (err: any) => {
        this.loading = false;
        console.error('Full Error:', err);
        console.error('Error Status:', err.status);
        console.error('Error Body:', err.error);
        
        // Backend se aane wale different error formats handle karo
        if (err.error) {
          if (typeof err.error === 'string') {
            // Direct string error message
            alert(err.error);
          } else if (err.error.message) {
            // Object with message property
            alert(err.error.message);
          } else if (err.error.errors) {
            // Identity errors array (from ChangePasswordAsync)
            if (Array.isArray(err.error.errors)) {
              const errorMessages = err.error.errors.map((e: any) => e.description || e.code).join('\n');
              alert('Errors:\n' + errorMessages);
            } else {
              // Validation errors object
              const errors = Object.values(err.error.errors).flat();
              alert('Validation errors:\n' + errors.join('\n'));
            }
          } else {
            alert('Profile update failed. Please try again.');
          }
        } else {
          alert('Profile update failed. Please check your connection.');
        }
      }
    });
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      const base64 = reader.result as string;
      console.log(base64);
      
      this.profileForm.patchValue({
        profileImage: base64
      });
    };
    reader.readAsDataURL(file);
  }
}