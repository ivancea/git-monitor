import React from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./NavMenu";

type Props = {
    children?: React.ReactNode;
};

export function Layout(props: Props): React.ReactElement {
    return (
        <div>
            <NavMenu />
            <Container>{props.children}</Container>
        </div>
    );
}
