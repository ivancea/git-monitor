import React, { useCallback, useState } from "react";
import { ListGroup } from "reactstrap";
import { ChangeObjectType, ChangeType, ChangeWrapper } from "../../types/changes";
import { Change } from "./Change";
import { ChangeFilterSelector } from "./ChangeFilterSelector";

type Props = {
    changes: ChangeWrapper[];
};

type Filter = {
    repositories: Map<string, boolean>;
    users: Map<string, boolean>;
    types: Map<string, boolean>;
    objectTypes: Map<string, boolean>;
};

const changeTypes = Object.keys(ChangeType).filter((t) => typeof t === "string");
const changeObjectTypes = Object.keys(ChangeObjectType).filter((t) => typeof t === "string");

export function ChangesList({ changes }: Props): React.ReactElement {
    const [filter, setFilter] = useState<Filter>({
        repositories: new Map(),
        users: new Map(),
        types: new Map(),
        objectTypes: new Map(),
    });

    const isHidden = (change: ChangeWrapper): boolean => {
        if (!filter.repositories.get(change.repository)) {
            return true;
        } else if (!filter.users.get(change.change.user?.name ?? "")) {
            return true;
        } else if (!filter.types.get(change.change.type)) {
            return true;
        } else if (!filter.objectTypes.get(change.change.objectType)) {
            return true;
        }

        return false;
    };

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

    return (
        <>
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
                onChanged={onUsersFilterChanged}
            />
            <ChangeFilterSelector
                name="Types"
                defaultOptions={changeTypes}
                accessor={useCallback((c) => c.change.type, [])}
                onChanged={onTypesFilterChanged}
            />
            <ChangeFilterSelector
                name="Object types"
                defaultOptions={changeObjectTypes}
                accessor={useCallback((c) => c.change.objectType, [])}
                onChanged={onObjectTypesFilterChanged}
            />
            <ListGroup>
                {changes
                    .map((change) => <Change key={change.id} change={change} hidden={isHidden(change)} />)
                    .reverse()}
            </ListGroup>
        </>
    );
}
