import { Observable } from "rxjs";
import { ILoginInformation } from "./ILoginInformation";
import { ApiResponse } from "../../commons/ApiResponse";

export interface ILoginService {
    login(login: ILoginInformation): Observable<ApiResponse<any>>;
}