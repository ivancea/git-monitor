import React, { useEffect, useState } from "react";
import { config } from "../Config";
import * as signalR from "@microsoft/signalr";
import { Alert, Button } from "reactstrap";
import { cloneDeep, isNil } from "lodash";
import { ChangeObjectType, Changes, ChangeType, ChangeWrapper, CommitChange } from "../types/changes";
import { ChangesList } from "./changes/ChangesList";
import { as } from "../utils";

export function Home(): React.ReactElement {
    const [error, setError] = useState<string>();
    const [changes, setChanges] = useState<ChangeWrapper[]>([
        {
            repository: "Test repository",
            seen: false,
            date: new Date(),
            change: as<CommitChange>({
                objectType: ChangeObjectType.Commit,
                type: ChangeType.Created,
                objectName: "Message",
                user: {
                    name: "Iván Cea Fontenla",
                    email: "ivancea96@outlook.com",
                },
                hash: "CommitSHA",
                message: "Message\nLong message",
            }),
        },
    ]);

    useEffect(() => {
        Notification.requestPermission();

        setError("Connecting...");

        const newHub = new signalR.HubConnectionBuilder().withUrl(config.url.API + "hubs/changes").build();

        newHub.on("changes", (newChanges: string) => {
            const wrappedChanges = Object.entries(JSON.parse(newChanges) as Changes).flatMap((e) =>
                e[1].map<ChangeWrapper>((c) => ({
                    repository: e[0],
                    date: new Date(),
                    change: c,
                    seen: false,
                })),
            );

            new Notification(wrappedChanges.length + " new changes");

            setChanges((c) => [...c, ...wrappedChanges]);
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
    }, []);

    const toggleError = React.useCallback(() => setError(undefined), [setError]);

    const markAllAsRead = React.useCallback(() => {
        setChanges((oldChanges) => oldChanges.map((change) => ({ ...cloneDeep(change), seen: true })));
    }, [setChanges]);

    return (
        <div>
            <Alert color="danger" isOpen={!isNil(error)} toggle={toggleError}>
                {error}
            </Alert>
            <Button color="success" onClick={markAllAsRead}>
                Mark all as read
            </Button>
            <ChangesList changes={changes} />
        </div>
    );
}
