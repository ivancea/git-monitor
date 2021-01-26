import { Dispatch, SetStateAction, useCallback, useEffect, useMemo, useState } from "react";
import * as uuid from "uuid";
import { ChangeWrapper, ChangesNotification } from "../types/changes";
import { config } from "../Config";
import { useLocalStorage } from "./useLocalStorage";
import { useSignalR } from "./useSignalR";

export type Filter = {
    repositories: Map<string, boolean>;
    users: Map<string, boolean>;
    types: Map<string, boolean>;
    objectTypes: Map<string, boolean>;
};

export type ChangesData = {
    connectionError?: string;
    setConnectionError: Dispatch<SetStateAction<string | undefined>>;
    changes: ChangeWrapper[];
    setChanges: Dispatch<SetStateAction<ChangeWrapper[]>>;
    errors: ChangesNotification["errors"];
    setErrors: Dispatch<SetStateAction<ChangesNotification["errors"]>>;
    filter: Filter;
    setFilter: Dispatch<SetStateAction<Filter>>;
    isHidden: (change: ChangeWrapper) => boolean;
    notifyHiddenChanges: boolean;
    setNotifyHiddenChanges: Dispatch<SetStateAction<boolean>>;
};

export function useChanges(): ChangesData {
    const [connectionError, setConnectionError] = useState<string>();
    const [changes, setChanges] = useLocalStorage<ChangeWrapper[]>("changes", []);
    const [errors, setErrors] = useState<ChangesNotification["errors"]>({});
    const [filter, setFilter] = useState<Filter>({
        repositories: new Map(),
        users: new Map(),
        types: new Map(),
        objectTypes: new Map(),
    });
    const [notifyHiddenChanges, setNotifyHiddenChanges] = useState(true);

    const isHidden = useCallback(
        (change: ChangeWrapper): boolean => {
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
        },
        [filter],
    );

    useEffect(() => {
        Notification.requestPermission();
    }, []);

    useSignalR(
        config.url.API + "hubs/changes",
        setConnectionError,
        useMemo(
            () => ({
                changes: [
                    (newChangesJson: unknown) => {
                        const newChanges: ChangesNotification = JSON.parse(newChangesJson as string);
                        const wrappedChanges = Object.entries(newChanges.changes).flatMap((e) =>
                            e[1].map<ChangeWrapper>((c) => ({
                                id: uuid.v4(),
                                repository: e[0],
                                date: new Date().toLocaleTimeString(),
                                change: c,
                                seen: false,
                            })),
                        );

                        if (wrappedChanges.length > 0) {
                            const changesToNotify = notifyHiddenChanges
                                ? wrappedChanges
                                : wrappedChanges.filter((c) => !isHidden(c));

                            if (changesToNotify.length > 0) {
                                new Notification(
                                    changesToNotify.length +
                                        " new change" +
                                        (wrappedChanges.length > 1 ? "s" : "") +
                                        " in " +
                                        Array.from(new Set(changesToNotify.map((c) => c.repository)))
                                            .sort()
                                            .join(", "),
                                );
                            }

                            setChanges((c) => [...c, ...wrappedChanges]);
                        }

                        setErrors(newChanges.errors);
                    },
                ],
            }),
            [setChanges, isHidden, notifyHiddenChanges],
        ),
    );

    return {
        connectionError,
        setConnectionError,
        changes,
        setChanges,
        errors,
        setErrors,
        filter,
        setFilter,
        isHidden,
        notifyHiddenChanges,
        setNotifyHiddenChanges,
    };
}
