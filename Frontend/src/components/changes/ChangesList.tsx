import { cloneDeep } from "lodash";
import React, { Dispatch, SetStateAction, useCallback } from "react";
import { Button, ButtonGroup, Collapse, ListGroup } from "reactstrap";
import { Filter } from "../../hooks/useChanges";
import { ChangeObjectType, ChangeType, ChangeWrapper } from "../../types/changes";
import { ChangeFilterSelector } from "./ChangeFilterSelector";
import { Change } from "./Change";

type Props = {
    changes: ChangeWrapper[];
    setChanges: Dispatch<SetStateAction<ChangeWrapper[]>>;
    setFilter: Dispatch<SetStateAction<Filter>>;
    isHidden: (change: ChangeWrapper) => boolean;
    notifyHiddenChanges: boolean;
    setNotifyHiddenChanges: Dispatch<SetStateAction<boolean>>;
};

const changeTypes = Object.keys(ChangeType).filter((t) => typeof t === "string");
const changeObjectTypes = Object.keys(ChangeObjectType).filter((t) => typeof t === "string");

export function ChangesList({
    changes,
    setChanges,
    setFilter,
    isHidden,
    notifyHiddenChanges,
    setNotifyHiddenChanges,
}: Props): React.ReactElement {
    const useFilterChanged = (filterType: keyof Filter): ((elements: Map<string | undefined, boolean>) => void) =>
        useCallback(
            (filter: Map<string | undefined, boolean>) =>
                setFilter((f) => ({
                    ...f,
                    [filterType]: filter,
                })),
            [filterType],
        );

    const onRepositoriesFilterChanged = useFilterChanged("repositories");
    const onUsersFilterChanged = useFilterChanged("users");
    const onTypesFilterChanged = useFilterChanged("types");
    const onObjectTypesFilterChanged = useFilterChanged("objectTypes");

    const markAllAsRead = React.useCallback(() => {
        setChanges((oldChanges) => oldChanges.map((change) => ({ ...cloneDeep(change), seen: true })));
    }, [setChanges]);

    const removeAllReadChanges = React.useCallback(() => {
        setChanges((oldChanges) => oldChanges.filter((change) => !change.seen));
    }, [setChanges]);

    const toggleNotifyHiddenChanges = React.useCallback(() => {
        setNotifyHiddenChanges((oldNotifyHiddenChanges) => !oldNotifyHiddenChanges);
    }, [setNotifyHiddenChanges]);

    return (
        <>
            <ButtonGroup>
                <Button color="success" onClick={markAllAsRead}>
                    Mark all as read
                </Button>
                <Button color="danger" onClick={removeAllReadChanges}>
                    Remove all read changes
                </Button>
            </ButtonGroup>
            <Button
                className="ml-1"
                color={notifyHiddenChanges ? "success" : "secondary"}
                outline={!notifyHiddenChanges}
                onClick={toggleNotifyHiddenChanges}
            >
                {notifyHiddenChanges ? "✅" : "❌"} Notify hidden changes
            </Button>
                <ChangeFilterSelector
                    name="Repositories"
                    changes={changes}
                    accessor={useCallback((c) => c.repository, [])}
                    onChanged={onRepositoriesFilterChanged}
                />
                <ChangeFilterSelector
                    name="Users"
                    changes={changes}
                    accessor={useCallback((c) => (c.change.user ? c.change.user.name : ""), [])}
                    textSelector={useCallback((o) => (o === "" ? <i>No user</i> : o), [])}
                    onChanged={onUsersFilterChanged}
                />
                <ChangeFilterSelector
                    name="Types"
                    changes={changes}
                    defaultOptions={changeTypes}
                    accessor={useCallback((c) => c.change.type, [])}
                    onChanged={onTypesFilterChanged}
                />
                <ChangeFilterSelector
                    name="Object types"
                    changes={changes}
                    defaultOptions={changeObjectTypes}
                    accessor={useCallback((c) => c.change.objectType, [])}
                    onChanged={onObjectTypesFilterChanged}
                />
            Total: {changes.length}, Visible: {changes.filter((c) => !isHidden(c)).length}
            <ListGroup>
                {changes
                    .map((change) => <Change key={change.id} change={change} hidden={isHidden(change)} />)
                    .reverse()}
            </ListGroup>
        </>
    );
}
