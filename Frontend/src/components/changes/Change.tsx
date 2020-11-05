import React, { useEffect, useState } from "react";
import { Badge, Col, Collapse, ListGroupItem, ListGroupItemHeading, ListGroupItemText, Row } from "reactstrap";
import { ChangeObjectType, ChangeType, ChangeWrapper } from "../../types/changes";
import { ChangeDetails } from "./ChangeDetails";

type Props = {
    change: ChangeWrapper;
    hidden?: boolean;
};

export function Change({ change, hidden }: Props): React.ReactElement {
    const [collapsed, setCollapsed] = useState(false);

    useEffect(() => {
        setCollapsed(change.seen);
    }, [change.seen]);

    const toggleCollapsed = React.useCallback(() => setCollapsed((c) => !c), [setCollapsed]);

    return (
        <Collapse isOpen={!hidden}>
            <ListGroupItem
                className={"change" + (hidden ? " hidden-change" : "")}
                color={change.seen ? undefined : "success"}
            >
                <ListGroupItemHeading role="button" onClick={toggleCollapsed}>
                    <Row>
                        <Col xs={10}>
                            {renderChangeType(change.change.type, change.change.objectType)} {change.change.objectName}
                            <br />
                            {change.change.user ? (
                                <small>By: {change.change.user.name + " <" + change.change.user.email + ">"}</small>
                            ) : undefined}
                        </Col>
                        <Col xs={2} className="text-center">
                            <div>
                                <Badge color="info">{change.repository}</Badge>
                            </div>
                            <div>
                                <Badge color="secondary" pill>
                                    {change.date}
                                </Badge>
                            </div>
                        </Col>
                    </Row>
                </ListGroupItemHeading>
                <Collapse isOpen={!collapsed}>
                    <ListGroupItemText className="mb-0">
                        <ChangeDetails change={change} />
                    </ListGroupItemText>
                </Collapse>
            </ListGroupItem>
        </Collapse>
    );
}

function renderChangeType(changeType: ChangeType, changeObjectType: ChangeObjectType): React.ReactElement {
    switch (changeType) {
        case ChangeType.Created:
            return <Badge color="success">{changeType + " " + changeObjectType}</Badge>;
        case ChangeType.Updated:
            return <Badge color="primary">{changeType + " " + changeObjectType}</Badge>;
        case ChangeType.Deleted:
            return <Badge color="danger">{changeType + " " + changeObjectType}</Badge>;
    }
}
