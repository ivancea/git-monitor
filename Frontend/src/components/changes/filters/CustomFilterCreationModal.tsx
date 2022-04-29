import React from "react";
import { Modal, ModalBody, ModalHeader } from "reactstrap";
import { ChangeWrapper } from "../../../types/changes";

type Props = {
    onAddFilter: (accessor: (change: ChangeWrapper) => string, regex: RegExp) => void;
    onClose: () => void;
};

export default function CustomFilterCreationModal({ onAddFilter, onClose }: Props): React.ReactElement {
    return (
        <Modal isOpen={true} toggle={onClose} size="xs">
            <ModalHeader></ModalHeader>
            <ModalBody></ModalBody>
        </Modal>
    );
}
