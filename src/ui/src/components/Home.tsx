import React from 'react';
import { GithubPanel } from './github/GithubPanel';
import { JiraPanel } from './jira/JiraPanel';

type HomeProps = {
    stories: []
}

type HomeState = {
    stories: [],
    edit: string,
    redirect: boolean
}

export class Home extends React.Component<HomeProps, HomeState> {

    render() {
        return (
            <>
                <div className="row mt-3">
                    <div className="col-sm-6">
                        <GithubPanel />
                    </div>
                    <div className="col-sm-6">
                        <JiraPanel />
                    </div>
                </div>
            </>
        );
    }
}