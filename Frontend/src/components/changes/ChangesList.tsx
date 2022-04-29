import { cloneDeep } from "lodash";
import React, { Dispatch, SetStateAction } from "react";
import { Button, ButtonGroup, ListGroup } from "reactstrap";
import { ChangeWrapper } from "../../types/changes";
import { Change } from "./Change";

type Props = {
    changes: ChangeWrapper[];
    setChanges: Dispatch<SetStateAction<ChangeWrapper[]>>;
    isHidden: (change: ChangeWrapper) => boolean;
    notifyHiddenChanges: boolean;
    setNotifyHiddenChanges: Dispatch<SetStateAction<boolean>>;
};

export function ChangesList({
    changes,
    setChanges,
    isHidden,
    notifyHiddenChanges,
    setNotifyHiddenChanges,
}: Props): React.ReactElement {
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
            <div>
                Total: {changes.length}, Visible: {changes.filter((c) => !isHidden(c)).length}
            </div>
            <ListGroup>
                {changes
                    .map((change) => <Change key={change.id} change={change} hidden={isHidden(change)} />)
                    .reverse()}
            </ListGroup>
        </>
    );
}
