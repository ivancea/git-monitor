import React from "react";
import { Alert } from "reactstrap";
import { useChanges } from "../hooks/useChanges";
import { ChangesList } from "./changes/ChangesList";
import "./styles/global.scss";
import { RepositoryErrors } from "./RepositoryErrors";

export function Home(): React.ReactElement {
    const {
        connectionError,
        setConnectionError,
        changes,
        setChanges,
        errors,
        setFilter,
        isHidden,
        notifyHiddenChanges,
        setNotifyHiddenChanges,
    } = useChanges();

    const toggleError = React.useCallback(() => setConnectionError(undefined), [setConnectionError]);

    return (
        <div>
            <Alert color="danger" isOpen={!!connectionError} toggle={toggleError}>
                {connectionError}
            </Alert>
            <RepositoryErrors errors={errors} />
            <ChangesList
                changes={changes}
                setChanges={setChanges}
                setFilter={setFilter}
                isHidden={isHidden}
                notifyHiddenChanges={notifyHiddenChanges}
                setNotifyHiddenChanges={setNotifyHiddenChanges}
            />
        </div>
    );
}
