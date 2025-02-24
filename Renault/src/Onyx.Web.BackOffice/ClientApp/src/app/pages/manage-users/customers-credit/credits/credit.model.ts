
export interface CreditModel {
    id: number;
    date: string;
    value: string;
    modifierUserName: string;
    modifierUserId: string;
    orderToken: string | undefined;
}
