interface LoginModel {
    email: string;
    password: string;
}
interface RegisterModel {
    userName: string;
    email: string;
    phone: string;
    password: string;
}
interface User {
    id: string;
    name: string;
    surname: string;
    email: string;
    userName: string;
    token: string;
}
interface KeyValueModel {
    key: any;
    value: any;
}
