import React from 'react';
import SprintTask from './models/SprintTask';

type JiraGroupProps = {
    jiraTasks: Array<SprintTask>
}
type JiraGroupState = {}

export class JiraGroup extends React.Component<JiraGroupProps, JiraGroupState> {
    render() {
        let self = this
        let currentGroup = this.props.jiraTasks[0].fields.status.name
        console.log(this.props.jiraTasks)
        return (
            <div className="Box mb-3">
                <div className="Box--condensed box-header box-title">{currentGroup}</div>
                <ul className="no-mb box-item">
                    {self.props.jiraTasks.map((task, i) => {

                        let assignee = <div></div>
                        if (task.fields.assignee)
                            assignee = <img width="40px" height="40px" src={task.fields.assignee.avatarUrls['48x48']} title={task.fields.assignee.displayName} alt="" className="pr-avatar" />

                        let epicLabel = <span className="no-fix">no epic</span>
                        if (task.fields.epic)
                            epicLabel = <a href={'https://checkout.atlassian.net/browse/' + task.fields.epic.key}>{task.fields.epic.name}</a>

                        let fixLabel = <span className="no-fix">no fix version</span>
                        if (task.fields.fixVersion)
                            fixLabel =
                                <div>
                                    {task.fields.fixVersion.map((version) => <span><a href={'https://checkout.atlassian.net/projects/TDS2/versions/' + version.id}>{version.name}</a></span>).join(', ')}
                                </div>

                        let storyPoints: any = 'n/a'
                        if (task.fields.customfield_10124)
                            storyPoints = task.fields.customfield_10124

                        return (
                            <li key={i} className="jira-task-container Box-row">
                                <p className="pr-title">
                                    <img src={task.fields.issueType.iconUrl} title={task.fields.issueType.name} alt="" /> [{storyPoints}] <a href={task.url}>{task.key} / {task.fields.summary}</a>
                                </p>
                                <div>
                                    {assignee}
                                    <div className="jira-epic-fix">
                                        <div className="smaller-text">{epicLabel}</div>
                                        <div className="smaller-text">{fixLabel}</div>
                                    </div>
                                </div>
                            </li>
                        )
                    })}
                </ul>
            </div>
        )
    }
}