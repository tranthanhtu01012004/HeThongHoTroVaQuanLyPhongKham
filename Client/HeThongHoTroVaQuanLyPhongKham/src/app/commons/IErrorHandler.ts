import { HttpErrorResponse } from "@angular/common/http";

export interface IErrorHandler {
    handleError(error: HttpErrorResponse): string[];
}