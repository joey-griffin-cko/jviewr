import User from "./User";

export default class Review {
    constructor(user: User, state: string) {
        this.user = user;
        this.state = state;
    }
    user: User;
    state: string;
}