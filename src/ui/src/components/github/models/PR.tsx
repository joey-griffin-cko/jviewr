import Review from "./Review";
import User from "./User";

export default class PR {
    constructor(repo: string, repoUrl: string, title: string, number: number, html_url: string, body: string, user: User, reviews: Array<Review>, hasApprovals: boolean) {
        this.repo = repo
        this.repoUrl = repoUrl
        this.title = title
        this.number = number
        this.html_url = html_url
        this.body = body
        this.user = user
        this.reviews = reviews
        this.hasApprovals = hasApprovals
    }
    repo: string
    repoUrl: string
    title: string
    number: number
    html_url: string
    body: string
    user: User
    reviews: Array<Review>
    hasApprovals: boolean
}