import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-export-excel-modal",
  templateUrl: "./export-excel-modal.component.html",
  styleUrls: ["./export-excel-modal.component.scss"],
})
export class ExportExcelModalComponent {
  inProgressAllExportbtn = false;
  inProgressExportForm = false;
  exportForm!: FormGroup;
  @Input() client: any;
  @Input() clientQuery: string = "";

  constructor(private exportService: ExportFileService) {}

  ngOnInit(): void {
    this.exportForm = this.exportService.exportForm();
  }
  ngOnDestroy(): void {
    this.handleCloseFormModal(this.exportForm)
  }

  submitExportFormOrder() {
    this.inProgressExportForm = true;
    this.exportService
      .submitExportForm(this.exportForm, this.client, this.clientQuery)
      .subscribe({
        next: (response: any) => {
          this.exportService.exportFile(response);
          this.inProgressExportForm = false;
        },
        error: () => {
          this.inProgressExportForm = false;
        },
      });
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }
}
