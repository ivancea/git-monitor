import React, { useEffect, useState } from "react";
import {
  Badge,
  Collapse,
  ListGroupItem,
  ListGroupItemHeading,
  ListGroupItemText,
} from "reactstrap";
import {
  ChangeObjectType,
  ChangeType,
  ChangeWrapper,
} from "../../types/changes";

type Props = {
  change: ChangeWrapper;
};

export function Change({ change }: Props): React.ReactElement {
  const [collapsed, setCollapsed] = useState(change.seen);

  const toggleCollapsed = React.useCallback(() => setCollapsed((c) => !c), [
    setCollapsed,
  ]);

  useEffect(() => {
    if (change.seen) {
      setCollapsed(true);
    }
  }, [change.seen]);

  return (
    <ListGroupItem color={change.seen ? undefined : "success"}>
      <ListGroupItemHeading className="change-header" onClick={toggleCollapsed}>
        <Badge color="info">{change.repository}</Badge>{" "}
        {renderChangeType(change.change.type, change.change.objectType)}{" "}
        {change.change.objectName}
      </ListGroupItemHeading>
      <Collapse isOpen={!collapsed}>
        <ListGroupItemText>Test</ListGroupItemText>
      </Collapse>
    </ListGroupItem>
  );
}

function renderChangeType(
  changeType: ChangeType,
  changeObjectType: ChangeObjectType
): React.ReactElement {
  switch (changeType) {
    case ChangeType.Created:
      return (
        <Badge color="success">{changeType + " " + changeObjectType}</Badge>
      );
    case ChangeType.Updated:
      return (
        <Badge color="primary">{changeType + " " + changeObjectType}</Badge>
      );
    case ChangeType.Deleted:
      return (
        <Badge color="danger">{changeType + " " + changeObjectType}</Badge>
      );
  }
}
