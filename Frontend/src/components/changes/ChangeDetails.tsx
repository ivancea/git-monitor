import React from 'react';
import { Change, isBranchChange, isCommitChange, isTagChange } from '../../types/changes';

type Props = {
    change: Change;
};

export function ChangeDetails({ change }: Props): React.ReactElement {
    if (isCommitChange(change)) {
        return (
            <ul className="change-details">
                <li>
                    Hash: <pre>{change.hash}</pre>
                </li>
                <li>
                    Message: <pre>{change.message}</pre>
                </li>
            </ul>
        );
    }

    if (isBranchChange(change)) {
        return (
            <ul className="change-details">
                <li>
                    Target commit: <pre>{change.targetCommit}</pre>
                </li>
            </ul>
        );
    }

    if (isTagChange(change)) {
        return (
            <ul className="change-details">
                <li>
                    Target commit: <pre>{change.targetCommit}</pre>
                </li>
                {change.message ? (
                    <li>
                        Message: <pre>{change.message}</pre>
                    </li>
                ) : undefined}
            </ul>
        );
    }

    return <></>;
}
