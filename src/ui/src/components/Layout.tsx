import React from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

type LayoutProps = { };
type LayoutState = { };

export class Layout extends React.Component<LayoutProps, LayoutState> {
    render () {
        return (
            <div>
                <NavMenu />
                <Container fluid={true}>
                    {this.props.children}
                </Container>
            </div>
        )
    }
}