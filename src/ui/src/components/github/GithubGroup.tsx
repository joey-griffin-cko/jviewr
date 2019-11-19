import React from 'react';
import PR from './models/PR';

type GithubGroupProps = {
    openPRs: Array<PR>
}
type GithubGroupState = {}

export class GithubGroup extends React.Component<GithubGroupProps, GithubGroupState> {

    render() {
        let self = this
        let repoUrl = `https://github.com/${this.props.openPRs[0].repo}`
        return (
            <div>
                <div className="Box mb-3">
                    <div className="Box--condensed box-header box-title"><a href={repoUrl}>{this.props.openPRs[0].repo}</a></div>
                    <ul className="no-mb box-item">
                        {self.props.openPRs.map((pr, i) => {
                            let approvalContent
                            if (pr.hasApprovals) {
                                approvalContent = <>
                                    <span className="smaller-text" key={i}>Approved by </span>
                                    {
                                        pr.reviews.map((review, j) => {
                                            return (
                                                <span key={j}><img src={review.user.avatar_url} alt={review.user.login} title={review.user.login}
                                                    className="rounded-avatar" /></span>
                                            )
                                        })
                                    }
                                </>
                            } else {
                                approvalContent = <span className="smaller-text no-fix">Pending approvals</span>
                            }
                            return (
                                <li key={i} className="image-txt-container Box-row">
                                    <p className="pr-title">
                                        <a href={pr.html_url}>{pr.title}</a>
                                    </p>
                                    <div>
                                        <img width="40px" height="40px" src={pr.user.avatar_url} alt={pr.user.login} title={pr.user.login} className="pr-avatar " />
                                        <div className="github-text">{approvalContent}</div>
                                    </div>
                                </li>
                            )
                        })}
                    </ul>
                </div>
            </div >
        )
    }
}