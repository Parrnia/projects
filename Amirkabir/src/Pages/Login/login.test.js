import Login from './login';
import React, { act } from 'react';
import Validation from './loginValidation';
import { BrowserRouter } from 'react-router-dom';
import { LoginServices } from '../../Services/loginServices';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: () => mockNavigate,
}));

describe('Login Component', () => {
    beforeEach(() => {
        localStorage.clear();
        jest.clearAllMocks();
    });

    describe('Validation Implementation', () => {
        test('validates empty fields correctly', () => {
            const [errors, isValid] = Validation({ emailOrPhone: '', password: '' });
            expect(isValid).toBe(false);
            expect(errors.emailOrPhone).toBe('فیلد الزامی ');
            expect(errors.password).toBe('فیلد الزامی ');
        });

        test('validates invalid input format', () => {
            const [errors, isValid] = Validation({ 
                emailOrPhone: 'invalid-input', 
                password: 'validPass123' 
            });
            expect(isValid).toBe(false);
            expect(errors.emailOrPhone).toBe('ایمیل یا شماره تلفن نامعتبر است');
        });

        test('validates password length correctly', () => {
            const [errors, isValid] = Validation({ 
                emailOrPhone: 'valid@email.com', 
                password: '123' 
            });
            expect(isValid).toBe(false);
            expect(errors.password).toBe('پسورد نامعتبر است');
        });
    });

    describe('Login Service Implementation', () => {
        test('successful login service call', async () => {
            const credentials = {
                username: 'hosnakazemian@gmail.com',
                password: 'Q123456'
            };

            try 
            {
                const response = await LoginServices(credentials);
                expect(response.status).toBe(200);
                expect(response.data).toBeTruthy();
            } 
            catch (error) {
                expect(error).toBeFalsy();
            }
        });

        test('failed login service call', async () => {
            const credentials = {
                username: 'wrong@email.com',
                password: 'wrongpass'
            };
            try 
            {
                await LoginServices(credentials);
                fail('Should have thrown an error');
            } 
            catch (error) {
                expect(error).toBeTruthy();
                expect(error.message).toBeTruthy();
            }
        });
    });

    describe('Integration Tests', () => {
        const renderLoginComponent = async () => {
            await act(async () => {
                render(
                    <BrowserRouter>
                        <Login />
                    </BrowserRouter>
                );
            });
        };

        test('successful login flow', async () => {
            await renderLoginComponent();
            const emailInput = screen.getByPlaceholderText('ایمیل یا شماره تلفن خود را وارد کنید');
            const passwordInput = screen.getByPlaceholderText('رمز خود را وارد کنید');
            const loginButton = screen.getByRole('button', { name: 'ورود' });

            await act(async () => {
                fireEvent.change(emailInput, { target: { value: 'hosnakazemian@gmail.com' } });
                fireEvent.change(passwordInput, { target: { value: 'Q123456' } });
                fireEvent.click(loginButton);
            });

            await waitFor(() => {
                expect(localStorage.getItem('isLogin')).toBe('1');
                expect(localStorage.getItem('token')).toBeTruthy();
                expect(localStorage.getItem('emailOrPhone')).toBe('hosnakazemian@gmail.com');
                expect(mockNavigate).toHaveBeenCalledWith('/profile');
            }, { timeout: 15000 });
        });

        test('failed login flow', async () => {
            await renderLoginComponent();
            const emailInput = screen.getByPlaceholderText('ایمیل یا شماره تلفن خود را وارد کنید');
            const passwordInput = screen.getByPlaceholderText('رمز خود را وارد کنید');
            const loginButton = screen.getByRole('button', { name: 'ورود' });

            await act(async () => {
                fireEvent.change(emailInput, { target: { value: 'wrong@email.com' } });
                fireEvent.change(passwordInput, { target: { value: 'wrongpass' } });
                fireEvent.click(loginButton);
            });

            await waitFor(() => {
                expect(localStorage.getItem('isLogin')).toBeFalsy();
                expect(localStorage.getItem('token')).toBeFalsy();
                expect(screen.getByText('پسورد نامعتبر است')).toBeInTheDocument();
            }, { timeout: 15000 });
        });
    });
});