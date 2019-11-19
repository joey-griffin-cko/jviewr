import JiraUser from "./JiraUser"

export default class Comment {
    constructor(author: JiraUser) {
        this.author = author
    }
    author: JiraUser
}