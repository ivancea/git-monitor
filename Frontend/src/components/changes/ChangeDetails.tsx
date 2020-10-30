import React, { useState } from "react";
import { Button } from "reactstrap";
import { ChangeWrapper, CommitChange, isBranchChange, isCommitChange, isTagChange } from "../../types/changes";
import { CommitDiffModal } from "./CommitDiffModal";

type Props = {
    change: ChangeWrapper;
};

export function ChangeDetails({ change }: Props): React.ReactElement {
    const [diffOpen, setDiffOpen] = useState(false);

    const toggleModal = React.useCallback(() => setDiffOpen((d) => !d), [setDiffOpen]);

    if (isCommitChange(change.change)) {
        return (
            <>
                <ul className="change-details">
                    <li>
                        Hash: <pre>{change.change.hash}</pre>
                    </li>
                    <li>
                        Message: <pre>{change.change.message}</pre>
                    </li>
                </ul>
                <Button size="sm" onClick={toggleModal}>
                    View diff
                </Button>
                {diffOpen ? (
                    <CommitDiffModal change={change as ChangeWrapper<CommitChange>} onClose={toggleModal} />
                ) : undefined}
            </>
        );
    }

    if (isBranchChange(change.change)) {
        return (
            <ul className="change-details">
                <li>
                    Target commit: <pre>{change.change.targetCommit}</pre>
                </li>
            </ul>
        );
    }

    if (isTagChange(change.change)) {
        return (
            <ul className="change-details">
                <li>
                    Target commit: <pre>{change.change.targetCommit}</pre>
                </li>
                {change.change.message ? (
                    <li>
                        Message: <pre>{change.change.message}</pre>
                    </li>
                ) : undefined}
            </ul>
        );
    }

    return <></>;
}
