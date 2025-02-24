import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { TeamMembersClient } from 'src/app/web-api-client';

export class TeamMemberValidators {
    /**
     *
     */
    constructor() { }
    static validTeamMemberText(teamMembersClient: TeamMembersClient, teamMemberId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return teamMembersClient.isUniqueTeamMemberName(teamMemberId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}

