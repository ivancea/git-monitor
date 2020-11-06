import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { isNil } from "lodash";
import { useEffect, useRef, useState } from "react";

export function useSignalR(
    url: string,
    setConnectionError: (connectionError?: string) => void,
    callbacks: Record<string, Array<(...args: unknown[]) => void>>,
): void {
    const [hub, setHub] = useState<HubConnection>();
    const lastMethods = useRef<Record<string, Array<(...args: unknown[]) => void>>>();

    useEffect(() => {
        setConnectionError("Connecting...");

        const newHub = new HubConnectionBuilder().withUrl(url).build();

        setHub(newHub);

        newHub.onreconnecting((e) =>
            setConnectionError("Reconnecting to the server..." + (e ? ` (${e.message})` : undefined)),
        );

        newHub.onclose(() => {
            setConnectionError("Disconnected from the server, reconnecting...");
            connect();
        });

        connect();

        function connect(): void {
            newHub
                .start()
                .then(() => setConnectionError(undefined))
                .catch((e) => {
                    setConnectionError("Error connecting to the server: " + JSON.stringify(e));

                    setTimeout(connect, 5000);
                });
        }

        return () => {
            newHub.stop();
            setHub(undefined);
        };
    }, [url, setConnectionError, setHub]);

    useEffect(() => {
        if (!isNil(hub)) {
            if (!isNil(lastMethods.current)) {
                Object.keys(lastMethods.current).forEach((method) =>
                    lastMethods.current?.[method].forEach((callback) => hub.off(method, callback)),
                );
            }

            lastMethods.current = callbacks;

            Object.keys(callbacks).forEach((method) =>
                callbacks[method].forEach((callback) => hub.on(method, callback)),
            );
        }
    }, [hub, callbacks]);
}
