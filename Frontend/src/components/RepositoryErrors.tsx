import React, { useState } from "react";
import { Alert, Badge, Collapse } from "reactstrap";
import { ChangesNotification } from "../types/changes";

type Props = {
    errors: ChangesNotification["errors"];
};

export function RepositoryErrors({ errors }: Props): React.ReactElement {
    const [collapsed, setCollapsed] = useState(false);

    const toggle = React.useCallback(() => setCollapsed((c) => !c), [setCollapsed]);

    if (Object.keys(errors).length == 0) {
        return <></>;
    }

    return (
        <Alert isOpen={true} color="warning">
            <div role="button" tabIndex={0} onClick={toggle}>
                <h4>Repository errors</h4>
            </div>
            <Collapse isOpen={!collapsed}>
                {Object.keys(errors).map((repository) => (
                    <Alert key={repository} color="danger" isOpen={true}>
                        <h5>
                            <Badge color="info">{repository}</Badge> {errors[repository]}
                        </h5>
                    </Alert>
                ))}
            </Collapse>
        </Alert>
    );
}
