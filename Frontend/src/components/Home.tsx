import React from "react";
import { Alert } from "reactstrap";
import { useChanges } from "../hooks/useChanges";
import { ChangesList } from "./changes/ChangesList";
import { ChangesFilters } from "./changes/filters/ChangesFilters";
import { RepositoryErrors } from "./RepositoryErrors";
import "./styles/global.scss";

export function Home(): React.ReactElement {
    const {
        connectionError,
        setConnectionError,
        changes,
        setChanges,
        errors,
        filter,
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
            <ChangesFilters changes={changes} filter={filter} setFilter={setFilter} />
            <ChangesList
                changes={changes}
                setChanges={setChanges}
                isHidden={isHidden}
                notifyHiddenChanges={notifyHiddenChanges}
                setNotifyHiddenChanges={setNotifyHiddenChanges}
            />
        </div>
    );
}
