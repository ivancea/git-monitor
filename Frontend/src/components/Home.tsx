import React, { useEffect, useState } from 'react';
import { config } from '../Config';
import * as signalR from '@microsoft/signalr';
import { Alert, Button, Spinner } from 'reactstrap';
import { cloneDeep, isNil } from 'lodash';
import { ChangeObjectType, Changes, ChangeType, ChangeWrapper, CommitChange } from '../types/changes';
import { ChangesList } from './changes/ChangesList';
import { as } from '../utils';

export function Home(): React.ReactElement {
    const [hub, setHub] = useState<signalR.HubConnection>();
    const [error, setError] = useState<string>();
    const [changes, setChanges] = useState<ChangeWrapper[]>([
        {
            repository: 'Test repository',
            seen: false,
            date: new Date(),
            change: as<CommitChange>({
                objectType: ChangeObjectType.Commit,
                type: ChangeType.Created,
                objectName: 'Message',
                user: {
                    name: 'Iván Cea Fontenla',
                    email: 'ivancea96@outlook.com',
                },
                hash: 'CommitSHA',
                message: 'Message\nLong message',
            }),
        },
    ]);

    useEffect(() => {
        Notification.requestPermission();

        const newHub = new signalR.HubConnectionBuilder().withUrl(config.url.API + 'hubs/changes').build();

        newHub.on('changes', (newChanges: Changes) => {
            const wrappedChanges = Object.entries(newChanges).flatMap((e) =>
                e[1].map<ChangeWrapper>((c) => ({
                    repository: e[0],
                    date: new Date(),
                    change: c,
                    seen: false,
                })),
            );

            new Notification(wrappedChanges.length + ' new changes');

            setChanges((c) => [...c, ...wrappedChanges]);
        });

        newHub
            .start()
            .then(() => setHub(newHub))
            .catch((e) => setError(e));
    }, []);

    const toggleError = React.useCallback(() => setError(undefined), [setError]);

    const markAllAsRead = React.useCallback(() => {
        setChanges((oldChanges) => oldChanges.map((change) => ({ ...cloneDeep(change), seen: true })));
    }, [setChanges]);

    if (isNil(hub) && !isNil(error)) {
        return <Spinner color="primary" />;
    }

    return (
        <div>
            <Alert color="danger" isOpen={!isNil(error)} toggle={toggleError}>
                {JSON.stringify(error, null, 4)}
            </Alert>
            <Button color="success" onClick={markAllAsRead}>
                Mark all as read
            </Button>
            <ChangesList changes={changes} />
        </div>
    );
}
