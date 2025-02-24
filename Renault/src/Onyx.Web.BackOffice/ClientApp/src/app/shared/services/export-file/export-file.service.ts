import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import * as moment from "moment-jalaali";
import { FileResponse } from 'src/app/web-api-client';

@Injectable({
  providedIn: "root",
})
export class ExportFileService {
  constructor(private fb: FormBuilder) {}

  exportForm(): FormGroup {
    return this.fb.group({
      searchText: "",
      pageNumber : 1,
      pageSize: null,
      startCreationDate: null,
      endCreationDate: null,
      startChangeDate: null,
      endChangeDate: null,
    });
  }

  submitExportForm(
    exportForm: FormGroup,
    exportClient: any,
    exportMethod: string
  ) {
    if (exportForm.valid) {
      const startCreationDate = this.formatDate(
        exportForm.controls["startCreationDate"].value
      );
      const endCreationDate = this.formatDate(
        exportForm.controls["endCreationDate"].value
      );
      const startChangeDate = this.formatDate(
        exportForm.controls["startChangeDate"].value
      );
      const endChangeDate = this.formatDate(
        exportForm.controls["endChangeDate"].value
      );
      debugger;
      return exportClient[exportMethod](
        exportForm.controls["searchText"].value,
        1,
        exportForm.controls["pageSize"].value,
        startCreationDate,
        endCreationDate,
        startChangeDate,
        endChangeDate
      );
    }
    return null;
  }

  private formatDate(date: any): Date | null {
    return date
      ? moment(date, "jYYYY/jMM/jDD").utcOffset(0, true).toDate()
      : null;
  }

  resetForm(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  exportFile(response: any) {
    const fileResponse = response as FileResponse;
    const url = window.URL.createObjectURL(fileResponse.data);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileResponse.fileName ?? "1";
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
    a.remove();
  }
}
