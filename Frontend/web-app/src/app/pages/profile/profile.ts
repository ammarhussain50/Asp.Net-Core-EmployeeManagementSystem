import { Component, inject } from '@angular/core';
import { Auth } from '../../services/auth';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule,],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  authService = inject(Auth);
  fb = inject(FormBuilder);

  profileForm!: FormGroup;
  profileImage: string | null = null;

  ngOnInit() {
    // form initialization
    this.profileForm = this.fb.group({
      FullName: ['', Validators.required],
      Email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
      PhoneNumber: ['']
    });

    // get profile data from API
    this.authService.getProfile().subscribe((result: any) => {
      console.log(result);

      // agar API image return kare to set karna
      this.profileImage = result.profileImage;

      // form me values patch karna
      this.profileForm.patchValue({
        FullName: result.fullName,
        Email: result.email,
        PhoneNumber: result.phoneNumber
      });
    });
  }

  // File select hone pe image ko preview ke liye set karna
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.profileImage = e.target.result; // base64 preview
      };
      reader.readAsDataURL(file);
    }
  }

  // save/update profile
  updateProfile() {
    if (this.profileForm.valid) {
      const updatedData = this.profileForm.getRawValue(); // email disabled h isliye getRawValue use
      console.log('Updated Data:', updatedData);

      // this.authService.updateProfile(updatedData).subscribe({
      //   next: (res) => {
      //     console.log('Profile updated successfully', res);
      //     alert('Profile updated successfully!');
      //   },
      //   error: (err) => {
      //     console.error('Error updating profile:', err);
      //   }
      // });
    }
  }
}
