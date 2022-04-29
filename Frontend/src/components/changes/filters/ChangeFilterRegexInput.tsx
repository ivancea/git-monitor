import { isNil } from "lodash";
import React, { useEffect, useState } from "react";
import { Col, FormFeedback, Input, Row } from "reactstrap";

type Props = {
    name: string;
    defaultValue?: string;
    onChanged: (value: RegExp) => void;
};

export function ChangeFilterRegexInput({ name, defaultValue, onChanged }: Props): React.ReactElement {
    const [text, setText] = useState(defaultValue ?? "^.*$");
    const [error, setError] = useState<string>();

    useEffect(() => {
        try {
            // Validate regex
            const regex = new RegExp(text);

            onChanged(regex);
            setError(undefined);
        } catch (e) {
            if (e instanceof SyntaxError) {
                setError("Invalid Regex: " + e.message[0].toUpperCase() + e.message.slice(1));
            } else {
                setError("Invalid Regex");
                console.error("Unknown regex error", e);
            }
        }
    }, [text, onChanged]);

    return (
        <Row>
            <Col className="text-right" xs="2">
                {name}:
            </Col>
            <Col xs="10">
                <Input invalid={!isNil(error)} type="text" value={text} onChange={(e) => setText(e.target.value)} />
                {!isNil(error) ? <FormFeedback>{error}</FormFeedback> : undefined}
            </Col>
        </Row>
    );
}
