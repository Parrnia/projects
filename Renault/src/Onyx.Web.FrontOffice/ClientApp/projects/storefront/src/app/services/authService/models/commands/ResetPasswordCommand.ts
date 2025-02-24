

export class ResetPasswordCommand {
  userId!: string;
  code!: string;
  newPassword!: string;
  confirmPassword!: string;
  captchaId!: string;
  captchaCode!: string;
}