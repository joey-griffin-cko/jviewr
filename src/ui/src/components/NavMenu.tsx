import React from 'react';

type NavProps = {

}

type NavState = {

}

export class NavMenu extends React.Component<NavProps, NavState> {

    render() {
        return (
            <ul className="nav">
                <li className="nav-item">
                    <a className="nav-link" href="/">Home</a>
                </li>
            </ul>
        );
    }
}