import React from 'react';
import PR from './models/PR';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap'
import { GithubListView } from './models/GithubListView';

let headers = { 'Content-Type': 'application/json' };

type GithubPanelProps = {}
type GithubPanelState = {
    openPRs: Array<Array<PR>>
    token: string
    canViewData: boolean
}

export class GithubPanel extends React.Component<GithubPanelProps, GithubPanelState> {
    constructor(props: GithubPanelProps) {
        super(props)
        this.getOpenPRs = this.getOpenPRs.bind(this)
        this.githubTokenOnChange = this.githubTokenOnChange.bind(this)
        this.submitGithubInfo = this.submitGithubInfo.bind(this)
        this.hasValidToken = this.hasValidToken.bind(this)
        this.state = {
            openPRs: new Array<Array<PR>>(),
            token: '',
            canViewData: false
        }
    }

    private intervalId: any

    componentWillUnmount() {
        clearInterval(this.intervalId);
    }

    getOpenPRs() {
        fetch('http://localhost:9341/api/github', {
            method: 'GET',
            headers: headers
        }).then(response => {
            if (response.status === 200)
                return response.json();

            this.setState({
                canViewData: false
            }, () => { throw new Error('getOpenPRs did not return 200.') })
        })
            .then((response) => {
                this.setState({ openPRs: response })
                this.intervalId = setTimeout(this.getOpenPRs.bind(this), 60000)
            })
            .catch(error => console.error(error));
    }

    githubTokenOnChange(event: any) {
        this.setState({ token: event.target.value })
    }

    hasValidToken() {
        fetch(`http://localhost:9341/api/github/status`, {
            method: 'GET',
            headers: headers
        }).then(response => {
            if (response.status === 200) {
                this.setState({
                    canViewData: true
                }, () => this.getOpenPRs())
            }
        })
    }

    componentDidMount() {
        this.hasValidToken();
    }

    submitGithubInfo() {
        fetch(`http://localhost:9341/api/github/${this.state.token}`, {
            method: 'POST',
            headers: headers
        }).then(response => {
            if (response.status === 200)
                return response;

            throw new Error('submitGithubInfo did not return 200.');
        })
            .then(() => {
                this.setState({
                    canViewData: true
                }, () => this.getOpenPRs())
            })
            .catch(error => console.error(error));
    }

    render() {

        let content
        if (this.state.canViewData) {
            content = <GithubListView openPRs={this.state.openPRs} />
        }
        else {
            content = <Form>
                <ul className="list-unstyled">
                    <li>Login to github.</li>
                    <li>Go to <a href="https://github.com/settings/tokens">token management</a>.</li>
                    <li>Click on "Generate new token".</li>
                    <li>Give it a note like <kbd>jirhub</kbd>.</li>
                    <li>Check the "repo" section.</li>
                    <li>Click on "Generate token" at the bottom of the page.</li>
                    <li>Copy the token into the textbox below.</li>
                </ul>
                <FormGroup>
                    <Label for="githubToken">Github Token</Label>
                    <Input type="text" name="githubToken" id="githubToken" onChange={this.githubTokenOnChange} />
                </FormGroup>
                <Button onClick={this.submitGithubInfo}>Submit</Button>
            </Form>
        }

        return (
            <div>
                {content}
            </div>
        )
    }
}