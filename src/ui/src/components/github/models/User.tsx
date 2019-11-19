export default class User {
    constructor(avatar_url: string, login: string) {
        this.avatar_url = avatar_url;
        this.login = login;
    }
    avatar_url: string;
    login: string;
}