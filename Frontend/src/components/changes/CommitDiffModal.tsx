import React, { useEffect, useState } from "react";
import { ListGroup, Modal, ModalBody, ModalHeader, Spinner } from "reactstrap";
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import { parseDiff } from "react-diff-view";
import { File } from "gitdiff-parser";
import { config } from "../../Config";
import { ChangeWrapper, CommitChange } from "../../types/changes";
import "react-diff-view/style/index.css";
import { FileDiff } from "./FileDiff";

type Props = {
    change: ChangeWrapper<CommitChange>;
    onClose: () => void;
};

export function CommitDiffModal({ change, onClose }: Props): React.ReactElement {
    const [diff, setDiff] = useState<File[]>();
    const toggle = React.useCallback(() => onClose(), [onClose]);

    useEffect(() => {
        let cancelled = false;

        fetch(
            config.url.API +
                `api/v1/repository/diff?repositoryName=${change.repository}&commitHash=${change.change.hash}`,
            { credentials: "include" },
        )
            .then((r) => r.text())
            .then((d) => cancelled || setDiff(parseDiff(d)?.filter((f: File) => f.oldPath !== "" || f.newPath !== "")))
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
        <Modal isOpen={true} toggle={toggle} size="xl">
            <ModalHeader toggle={toggle}>
                Diff of commit: <strong>{change.change.objectName}</strong> ({change.change.hash})
            </ModalHeader>
            <ModalBody className="commit-diff-body">
                {diff ? (
                    diff.length > 0 ? (
                        <ListGroup>
                            {diff.map((file: File, index: number) => (
                                <FileDiff key={index} file={file} />
                            ))}
                        </ListGroup>
                    ) : (
                        <h5>No changes</h5>
                    )
                ) : (
                    <Spinner />
                )}
            </ModalBody>
        </Modal>
    );
}
