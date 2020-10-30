import { File } from "gitdiff-parser";
import React, { useState } from "react";
import { Badge, Collapse, ListGroupItem, ListGroupItemHeading, ListGroupItemText } from "reactstrap";
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import { Diff } from "react-diff-view";

type Props = {
    file: File;
};

export function FileDiff({ file }: Props): React.ReactElement {
    const [collapsed, setCollapsed] = useState(true);

    const toggle = React.useCallback(() => setCollapsed((c) => !c), [setCollapsed]);

    return (
        <ListGroupItem>
            <ListGroupItemHeading className="file-diff-header" onClick={toggle}>
                {renderChangeHeader(file)}
            </ListGroupItemHeading>
            <ListGroupItemText>
                <Collapse isOpen={!collapsed}>
                    <Diff hunks={file.hunks} viewType="unified" />
                </Collapse>
            </ListGroupItemText>
        </ListGroupItem>
    );
}

function renderChangeHeader(file: File): React.ReactElement {
    switch (file.type) {
        case "add":
            return (
                <>
                    <Badge color="success">Added</Badge> {file.newPath}
                </>
            );
        case "modify":
            return (
                <>
                    <Badge color="primary">Modified</Badge> {file.newPath}
                </>
            );
        case "delete":
            return (
                <>
                    <Badge color="danger">Deleted</Badge> {file.oldPath}
                </>
            );
        case "copy":
            return (
                <>
                    <Badge color="warning">Copied</Badge> {file.oldPath} to {file.newPath}
                </>
            );
        case "rename":
            return (
                <>
                    <Badge color="secondary">Renamed</Badge> {file.oldPath} to {file.newPath}
                </>
            );
    }
}
