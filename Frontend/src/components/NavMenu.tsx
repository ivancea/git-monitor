import React from "react";
import { Link } from "react-router-dom";
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from "reactstrap";
import "./NavMenu.css";

type State = {
    collapsed: boolean;
};

export class NavMenu extends React.PureComponent<unknown, State> {
    constructor(props: unknown) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
        };
    }

    toggleNavbar(): void {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }

    render(): React.ReactElement {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">
                            Git monitor
                        </NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse
                            className="d-sm-inline-flex flex-sm-row-reverse"
                            isOpen={!this.state.collapsed}
                            navbar
                        >
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">
                                        Home
                                    </NavLink>
                                </NavItem>
                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
