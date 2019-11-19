import JiraStatus from "./JiraStatus";
import JiraUser from "./JiraUser";
import FixVersion from "./FixVersion";
import Epic from "./Epic";
import Comments from "./CommentList";
import IssueType from "./IssueType";

export default class JiraField {
    constructor(
        status: JiraStatus,
        summary: string,
        description: string,
        comment: Comments,
        issueType: IssueType,
        epic: Epic,
        assignee: JiraUser,
        customfield_10124: number,
        fixVersion: Array<FixVersion>
    ) {
        this.status = status
        this.summary = summary
        this.description = description
        this.comment = comment
        this.issueType = issueType
        this.epic = epic
        this.assignee = assignee
        this.customfield_10124 = customfield_10124
        this.fixVersion = fixVersion
    }

    status: JiraStatus
    summary: string
    description: string
    comment: Comments
    issueType: IssueType
    epic: Epic
    assignee: JiraUser
    customfield_10124: number
    fixVersion: Array<FixVersion>
}