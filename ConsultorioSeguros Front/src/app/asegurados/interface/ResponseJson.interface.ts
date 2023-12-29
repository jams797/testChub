import { Asegurados } from "./Asegurados.interface";

export interface ResponseJson {
    message: string;
    error: boolean;
    data: Array<Asegurados>;
}