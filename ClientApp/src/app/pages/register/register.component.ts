import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { BehaviorSubject, EMPTY, Observable, Subscription } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { IRegisterData } from 'src/app/interfaces/register-data';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnDestroy {
  private readonly subscription = new Subscription();
  private readonly trigger$ = new BehaviorSubject<any>(null);

  constructor(
    private readonly authService: AuthService,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router,
  ) {
    this.trigger$.next(null);
  }

  public form: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    repeatedPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    address: new FormControl('', [Validators.required]),
    dateOfBirth: new FormControl('', [Validators.required]),
    oib: new FormControl('', [
      Validators.required,
      Validators.pattern('^[0-9]{11}$'),
    ]),
  });

  public onFormSubmit(): void {
    
    if (
      this.form.get('password')?.value !==
      this.form.get('repeatedPassword')?.value
    ) {
      this.snackBar.open('Lozinke se moraju podudarati', 'Zatvori', {
        duration: 2000,
      });
      return;
    }

    if (this.form.invalid) {
      this.snackBar.open('Unesite sve potrebne podatke', 'Zatvori', {
        duration: 2000,
      });
      return;
    }

    const data: IRegisterData = {
      Id: 0,
      Mail: this.form.get('email')?.value,
      Lozinka: this.form.get('password')?.value,
      Ime: this.form.get('name')?.value,
      Prezime: this.form.get('surname')?.value,
      DatumRodenja: this.form.get('dateOfBirth')?.value,
      Adresa: this.form.get('address')?.value,
      Oib: this.form.get('oib')?.value.toString(),
      VrstaKorisnika: "K"
    };

    console.log(data);

    
    const registerSubscription = this.authService
      .register(data)
      .pipe(
        catchError(() => {
          this.snackBar.open('Unesite sve potrbene podatke', 'Zatvori', {
            duration: 2000,
          });
          return EMPTY;
        })
      )
      .subscribe(() => {
        this.router.navigate(['/']);
      });
    this.subscription.add(registerSubscription);
    
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  

  public test() {
    console.log(this.form.value);
  }
  
}
