

export interface RequestLogModel {
  id: number;
  apiAddress: string;
  requestBody: string | undefined ;
  errorMessage: string | undefined ;
  httpStatusCode: number;
  httpStatusCodeName: string;
  created: string;
  requestType: number;
  requestTypeName: string;
  apiTypeName: string;
}
