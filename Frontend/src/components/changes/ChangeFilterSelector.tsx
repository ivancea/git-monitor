import { isNil } from "lodash";
import React, { useCallback, useEffect, useState } from "react";
import { Badge, Col, Row } from "reactstrap";
import { ChangeWrapper } from "../../types/changes";

type Props = {
    name: string;
    changes?: ChangeWrapper[];
    defaultOptions?: string[];
    accessor: (c: ChangeWrapper) => string;
    textSelector?: (option: string) => React.ReactNode;
    onChanged: (elements: Map<string, boolean>) => void;
};

export function ChangeFilterSelector({
    name,
    changes,
    defaultOptions,
    accessor,
    textSelector,
    onChanged,
}: Props): React.ReactElement {
    const [options, setOptions] = useState(new Map<string, boolean>());

    useEffect(() => {
        const nextOptions = new Map<string, boolean>();

        if (!isNil(defaultOptions)) {
            defaultOptions.forEach((option) => nextOptions.set(option, true));
        }

        if (!isNil(changes)) {
            changes.forEach((change) => nextOptions.set(accessor(change), true));
        }

        setOptions((oldOptions) => {
            oldOptions.forEach((value, key) => {
                if (nextOptions.has(key)) {
                    nextOptions.set(key, value);
                }
            });

            onChanged(nextOptions);

            return nextOptions;
        });
    }, [changes, defaultOptions, accessor, onChanged]);

    const toggleOption = useCallback(
        (optionKey) => {
            setOptions((oldOptions) => {
                const nextOptions = new Map(oldOptions);

                nextOptions.set(optionKey, !oldOptions.get(optionKey));

                onChanged(nextOptions);

                return nextOptions;
            });
        },
        [setOptions, onChanged],
    );

    return (
        <Row>
            <Col className="filter-name" xs="2">
                {name}:{" "}
            </Col>
            <Col xs="10">
                {Array.from(options.keys())
                    .sort(compare)
                    .map((option) => (
                        <Badge
                            key={option}
                            className="filter-option-button"
                            color={options.get(option) ? "dark" : "secondary"}
                            onClick={() => toggleOption(option)}
                        >
                            {textSelector?.(option) ?? option}
                        </Badge>
                    ))}
            </Col>
        </Row>
    );
}

function compare(a: string, b: string): number {
    return a.toLocaleLowerCase().localeCompare(b.toLocaleLowerCase());
}
