import React from 'react';
import { JiraGroup } from './JiraGroup';
import SprintTask from './models/SprintTask';
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';

let headers = { 'Content-Type': 'application/json' };

type JiraPanelProps = {}
type JiraPanelState = {
    tasks: Array<Array<SprintTask>>
    token: string
    canViewData: boolean
}

export class JiraPanel extends React.Component<JiraPanelProps, JiraPanelState> {
    constructor(props: JiraPanelProps) {
        super(props)
        this.getJiraTask = this.getJiraTask.bind(this)
        this.jiraTokenOnChange = this.jiraTokenOnChange.bind(this)
        this.submitJiraInfo = this.submitJiraInfo.bind(this)
        this.hasValidToken = this.hasValidToken.bind(this)
        this.state = {
            tasks: new Array<Array<SprintTask>>(),
            token: '',
            canViewData: false
        }
    }

    getJiraTask() {
        fetch('http://localhost:9341/api/jira', {
            method: 'GET',
            headers: headers
        }).then(response => {
            if (response.status === 200)
                return response.json();

            throw new Error('getJiraTask did not return 200.');
        })
            .then((response) => {
                this.setState({ tasks: response })
            })
            .catch(error => console.error(error));
    }

    jiraTokenOnChange(event: any) {
        this.setState({ token: event.target.value })
    }

    hasValidToken() {
        fetch(`http://localhost:9341/api/jira/status`, {
            method: 'GET',
            headers: headers
        }).then(response => {
            if (response.status === 200) {
                this.setState({
                    canViewData: true
                }, () => this.getJiraTask())
            }
        })
    }

    componentDidMount() {
        this.hasValidToken();
    }

    submitJiraInfo() {
        fetch(`http://localhost:9341/api/jira/${this.state.token}`, {
            method: 'POST',
            headers: headers
        }).then(response => {
            if (response.status === 200)
                return response;

            throw new Error('submitJiraInfo did not return 200.');
        })
            .then(() => {
                this.setState({
                    canViewData: true
                }, () => this.getJiraTask())
            })
            .catch(error => console.error(error));
    }

    render() {

        let content
        if (this.state.canViewData) {
            content = <>
                {this.state.tasks.map((task, i) => {
                    return (
                        <JiraGroup key={i} jiraTasks={task} />
                    )
                })}</>
        }
        else {
            content = <Form>
                <ul className="list-unstyled">
                    <li>Login to Jira.</li>
                    <li>Go to <a href="https://id.atlassian.com/manage/api-tokens">token management</a>.</li>
                    <li>Click on "Create API token", and a modal will show.</li>
                    <li>Give the api token a name like <kbd>jirhub</kbd>.</li>
                    <li>Copy the token by clicking on "Copy to clipboard".</li>
                    <li>Submit the following into the textbox below: <kbd>tom.jones@checkout.com:&lt;token&gt;</kbd></li>
                </ul>
                <br />
                <FormGroup>
                    <Label for="jiraToken">Jira Token</Label>
                    <Input type="text" name="jiraToken" id="jiraToken" onChange={this.jiraTokenOnChange} />
                </FormGroup>
                <Button onClick={this.submitJiraInfo}>Submit</Button>
            </Form>
        }

        return (
            <div>
                {content}
            </div>
        )
    }
}