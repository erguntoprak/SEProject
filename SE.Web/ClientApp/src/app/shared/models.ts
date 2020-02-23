export interface LoginModel {
    email: string;
    password: string;
}
export interface RegisterModel {
    email: string;
    phone: string;
    password: string;
}
export interface User {
    id: string;
    name: string;
    surname: string;
    email: string;
    userName: string;
    token: string;
}
export interface KeyValueModel {
    key: any;
    value: any;
}
