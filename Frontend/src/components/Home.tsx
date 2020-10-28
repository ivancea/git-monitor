import React, { useEffect, useState } from "react";
import { config } from "../Config";
import * as signalR from "@microsoft/signalr";
import { Alert } from "reactstrap";
import { isNil } from "lodash";
import { Changes } from "../types/changes";

export function Home(): React.ReactElement {
  const [hub, setHub] = useState<signalR.HubConnection>();
  const [error, setError] = useState<string>();
  const [changes, setChanges] = useState<string>();

  useEffect(() => {
    const newHub = new signalR.HubConnectionBuilder()
      .withUrl(config.url.API + "hubs/changes")
      .build();

    newHub.on("changes", (newChanges: any) => setChanges(changes));

    newHub.start().catch((e) => setError(e));

    setHub(newHub);
  }, []);

  return (
    <div>
      <Alert
        color="danger"
        isOpen={!isNil(error)}
        toggle={React.useCallback(() => setError(undefined), [setError])}
      >
        {JSON.stringify(error, null, 4)}
      </Alert>
      <div>{JSON.stringify(changes, null, 4)}</div>
    </div>
  );
}
