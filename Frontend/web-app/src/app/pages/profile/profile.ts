import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Eye, EyeClosed, LoaderCircle, LucideAngularModule } from 'lucide-angular';
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
      newPassword: ['']
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
    this.httpService.getProfile(userId).subscribe({
      next: (res: IProfileResponse) => {
        this.profileForm.patchValue({
          name: res.name,
          phone: res.phone,
          profileImage: res.profileImage || ''
        });
      },
      error: () => {
        alert('Failed to load profile. Please try again.');
      }
    });
  }

  updateProfile() {
    this.loading = true;
    const formValue = this.profileForm.value;

    // Payload banate waqt sirf required + filled optional fields bhejo
    const payload: any = {
      userId: formValue.userId,
      name: formValue.name,
      phone: formValue.phone
    };

    if (formValue.profileImage) payload.profileImage = formValue.profileImage;
    if (formValue.oldPassword) payload.oldPassword = formValue.oldPassword;
    if (formValue.newPassword) payload.newPassword = formValue.newPassword;

    this.httpService.updateProfile(payload).subscribe({
      next: (res: any) => {
        this.loading = false;
        alert(res.message);
        this.router.navigate(['/']);
      },
      error: (err: any) => {
        this.loading = false;
        if (err.error && err.error.message) {
          alert(err.error.message);
        } else {
          alert('Profile update failed. Please try again.');
        }
      }
    });
  }
}
