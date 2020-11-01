import React, { useState } from "react";
import { Alert, Collapse } from "reactstrap";
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
        <div>
            <div className="errors-header" onClick={toggle}>
                Repository errors
            </div>
            <Collapse isOpen={!collapsed}>
                {Object.keys(errors).map((repository) => (
                    <Alert key={repository} color="danger" isOpen={true}>
                        {errors[repository]}
                    </Alert>
                ))}
            </Collapse>
        </div>
    );
}
