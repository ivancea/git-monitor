import React, { useEffect, useState } from "react";
import { Modal, ModalBody, ModalHeader, Spinner } from "reactstrap";
import { config } from "../../Config";
import { ChangeWrapper, CommitChange } from "../../types/changes";
import { parseDiff, Diff } from "react-diff-view";
import "react-diff-view/style/index.css";
import { File } from "gitdiff-parser";

type Props = {
    change: ChangeWrapper<CommitChange>;
    onClose: () => void;
};

export function CommitDiffModal({ change, onClose }: Props): React.ReactElement {
    const [diff, setDiff] = useState<File[]>();
    const toggle = React.useCallback(() => onClose(), [onClose]);

    console.log(diff);

    useEffect(() => {
        let cancelled = false;

        fetch(
            config.url.API +
                `api/v1/repository/diff?repositoryName=${change.repository}&commitHash=${change.change.hash}`,
        )
            .then((r) => r.text())
            .then((d) => cancelled || setDiff(parseDiff(d)))
            .catch((e) => {
                console.warn(e);
                cancelled || onClose();
            });

        return () => {
            cancelled = true;
            setDiff(undefined);
        };
    }, [setDiff, onClose, change.repository, change.change.hash]);

    return (
        <Modal isOpen={true} toggle={toggle} size="lg">
            <ModalHeader toggle={toggle}>
                Diff of commit: {change.change.objectName} ({change.change.hash})
            </ModalHeader>
            <ModalBody className="commit-diff-body">
                {diff ? (
                    <div>
                        {diff.map(({ hunks }: File, index: number) => (
                            <Diff key={index} hunks={hunks} viewType="unified" />
                        ))}
                    </div>
                ) : (
                    <Spinner />
                )}
            </ModalBody>
        </Modal>
    );
}
