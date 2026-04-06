import { Component, OnInit, signal, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { IftaLabelModule } from 'primeng/iftalabel';
import { AuthService } from '../../services/auth/auth.service';
import { IAuthToken } from '../../interface/authtoken.interfaces';
import { clearAuthToken } from '../../store/authToken/authToken.actions';
import { Store } from '@ngrx/store';

@Component({
    selector: 'app-login-modal',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DialogModule,
        InputTextModule,
        ButtonModule,
        PasswordModule,
        ToastModule,
        IftaLabelModule
    ],
    providers: [MessageService],
    templateUrl: './login.components.html',
    styleUrls: ['./login.components.scss']
})
export class LoginModalComponent implements OnInit {

    visible = signal(false);
    isLoading = signal(false);
    loginForm!: FormGroup;
    isAuthenticated = computed(() => !!this.authService.getToken());

    nomeUser = computed(() => {
        const payload = this.authService.getDecodedToken() as any;
        if (!payload) return '';
        return payload.name || 'Usuário';
    });

    private authService = inject(AuthService);
    private messageService = inject(MessageService);
    private fb = inject(FormBuilder);
    private store = inject(Store);

    ngOnInit() {
        this.initializeForm();
    }

    private initializeForm() {
        this.loginForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
    }

    logout() {
        clearAuthToken()
        this.store.dispatch(
            clearAuthToken()
        );
    }

    showLoginDialog() {
        this.visible.set(true);
    }

    closeDialog() {
        this.visible.set(false);
        this.loginForm.reset();
    }

    isFieldInvalid(fieldName: string): boolean {
        const field = this.loginForm.get(fieldName);
        return !!(field && field.invalid && field.touched);
    }

    onVisibleChange(event: boolean) {
        this.visible.set(event);
    }

    onSubmit() {
        if (this.loginForm.invalid) return;

        this.isLoading.set(true);

        const { email, password } = this.loginForm.value;

        this.authService.checkIn(email, password).subscribe({
            next: (response) => {
                this.isLoading.set(false);

                if (response.data) {
                    const parametros: IAuthToken = { token: response.data, type: '' };

                    this.authService.setToken(parametros);
                };

                this.closeDialog();
            },
            error: (error) => {
                this.isLoading.set(false);

                this.messageService.add({
                    severity: 'error',
                    summary: 'Erro',
                    detail: error.error?.message || 'Falha ao fazer login',
                    life: 3000
                });
            }
        });
    }

}