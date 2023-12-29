import { Seguros } from "./seguros";

export interface ResponseJson {
    message: string;
    error: boolean;
    data: Array<Seguros>;
}