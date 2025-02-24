

export class ResendActivationCodeCommand {
  userId?: string;
  captchaId!: string;
  captchaCode!: string;
}
