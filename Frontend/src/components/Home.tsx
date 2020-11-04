import React, { useEffect, useState } from "react";
import { config } from "../Config";
import { Alert, Button } from "reactstrap";
import { cloneDeep } from "lodash";
import { ChangesNotification, ChangeWrapper } from "../types/changes";
import { ChangesList } from "./changes/ChangesList";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "./styles/global.scss";
import { RepositoryErrors } from "./RepositoryErrors";
import { useLocalStorage } from "../hooks/useLocalStorage";
import * as uuid from "uuid";

export function Home(): React.ReactElement {
    const [error, setError] = useState<string>();
    const [changes, setChanges] = useLocalStorage<ChangeWrapper[]>("changes", []);
    const [errors, setErrors] = useState<ChangesNotification["errors"]>({});

    useEffect(() => {
        Notification.requestPermission();

        setError("Connecting...");

        const newHub = new HubConnectionBuilder().withUrl(config.url.API + "hubs/changes").build();

        newHub.on("changes", (newChangesJson: string) => {
            const newChanges: ChangesNotification = JSON.parse(newChangesJson);
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
                new Notification(
                    wrappedChanges.length +
                        " new change" +
                        (wrappedChanges.length > 1 ? "s" : "") +
                        " in " +
                        Object.keys(newChanges.changes).sort().join(", "),
                );

                setChanges((c) => [...c, ...wrappedChanges]);
            }

            setErrors(newChanges.errors);
        });

        newHub.onreconnecting((e) => setError("Reconnecting to the server..." + (e ? ` (${e.message})` : undefined)));

        newHub.onclose(() => {
            setError("Disconnected from the server, reconnecting...");
            connect();
        });

        connect();

        function connect(): void {
            newHub
                .start()
                .then(() => setError(undefined))
                .catch((e) => {
                    setError("Error connecting to the server: " + JSON.stringify(e));

                    setTimeout(connect, 5000);
                });
        }
    }, [setChanges]);

    const toggleError = React.useCallback(() => setError(undefined), [setError]);

    const markAllAsRead = React.useCallback(() => {
        setChanges((oldChanges) => oldChanges.map((change) => ({ ...cloneDeep(change), seen: true })));
    }, [setChanges]);

    return (
        <div>
            <Alert color="danger" isOpen={!!error} toggle={toggleError}>
                {error}
            </Alert>
            <RepositoryErrors errors={errors} />

            {changes.length == 0 ? (
                "No changes yet"
            ) : (
                <>
                    <Button color="success" onClick={markAllAsRead}>
                        Mark all as read
                    </Button>
                    <ChangesList changes={changes} />
                </>
            )}
        </div>
    );
}
