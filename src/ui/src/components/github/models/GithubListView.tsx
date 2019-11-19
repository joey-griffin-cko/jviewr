import React from 'react';
import PR from './PR';
import { GithubGroup } from '../GithubGroup';

type GithubListViewProps = {
    openPRs: Array<Array<PR>>
}
type GithubListViewState = {}

export class GithubListView extends React.Component<GithubListViewProps, GithubListViewState> {
    render() {
        return (
            <div>
                {this.props.openPRs
                    .map((group, i) => {
                        return (
                            <GithubGroup key={i} openPRs={group} />
                        )
                    })}
            </div>
        )
    }
}