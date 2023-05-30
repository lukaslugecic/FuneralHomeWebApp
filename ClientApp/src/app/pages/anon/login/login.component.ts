import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { catchError, EMPTY, Subscription } from 'rxjs';
import { ILoginData } from 'src/app/interfaces/login-data';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnDestroy {
  private subscription = new Subscription();
  hide = true;
  public error?: string;

  constructor(
    private readonly authService: AuthService,
    private readonly snackbar: MatSnackBar,
    private readonly router: Router
  ) {}

  public form: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  public onFormSubmit(): void {
    if (this.form.invalid) {
      this.snackbar.open('Neispravno korisničko ime ili lozinka!', 'Zatvori', { duration: 2000 });
      return;
    }

    const data: ILoginData = {
      mail: this.form.get('email')?.value as string,
      lozinka: this.form.get('password')?.value as string,
    };


    const loginSubscription = this.authService
      .login(data)
      .pipe(
        catchError(() => {
          this.snackbar.open('Neispravno korisničko ime ili lozinka!', 'Zatvori', { duration: 2000 });
          return EMPTY;
        })
      )
      .subscribe(() =>
        this.router.navigate(['/'])
      );

    this.subscription.add(loginSubscription);
  
  }
  

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
