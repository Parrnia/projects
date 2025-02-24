import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { QuestionsClient } from 'src/app/web-api-client';

export class QuestionValidators {
    /**
     *
     */
    constructor() { }
    static validQuestionText(questionsClient: QuestionsClient, questionId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const questionText = control.value;
            return questionsClient.isUniqueQuestionText(questionId, questionText).pipe(
                map(isAvailable => (isAvailable ? null : { questionTextTaken: true }))
            );
        }
    }
}

