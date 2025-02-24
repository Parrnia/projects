import { Component } from '@angular/core';
import { UrlService } from '../../../../services/url.service';
import { AllQuestionDto, QuestionType, QuestionsClient } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-page-faq',
    templateUrl: './page-faq.component.html',
    styleUrls: ['./page-faq.component.scss'],
})

export class PageFaqComponent {
    shippingQuestions : AllQuestionDto[] = [];
    paymentQuestions : AllQuestionDto[] = [];
    ordersandReturnsQuestions : AllQuestionDto[] = [];
    constructor(
        public url: UrlService,
        private questionsClient : QuestionsClient
    ) { 
        this.questionsClient.getAllQuestions().subscribe({
            next : (res) => {
                this.shippingQuestions = res.filter(f => f.questionType == QuestionType.ShippingInformation);
                this.paymentQuestions = res.filter(f => f.questionType == QuestionType.PaymentInformation);
                this.ordersandReturnsQuestions = res.filter(f => f.questionType == QuestionType.OrdersAndReturns);
            }}
        )

    }
}
