import { isNil } from "lodash";
import React, { useCallback, useEffect, useState } from "react";
import { Button, Col, Row } from "reactstrap";
import { ChangeWrapper } from "../../../types/changes";

type Props = {
    name: string;
    changes: ChangeWrapper[];
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
    const [counts, setCounts] = useState(new Map<string, number>());

    useEffect(() => {
        const nextOptions = new Map<string, boolean>();
        const nextCounts = new Map<string, number>();

        if (!isNil(defaultOptions)) {
            defaultOptions.forEach((option) => nextOptions.set(option, true));
        }

        if (!isNil(changes)) {
            changes.forEach((change) => {
                const value = accessor(change);
                nextOptions.set(value, true);
                nextCounts.set(value, (nextCounts.get(value) ?? 0) + 1);
            });
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
        setCounts(nextCounts);
    }, [changes, defaultOptions, accessor, onChanged]);

    const toggleOption = useCallback(
        (optionKey: string) => {
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
            <Col className="text-right" xs="2">
                {name}:
            </Col>
            <Col xs="10">
                {Array.from(options.keys())
                    .sort(compare)
                    .map((option) => (
                        <Button
                            key={option}
                            className="m-1 p-1"
                            color={options.get(option) ? "primary" : "secondary"}
                            outline={!options.get(option)}
                            onClick={() => toggleOption(option)}
                        >
                            {textSelector?.(option) ?? option} ({counts.get(option) ?? 0})
                        </Button>
                    ))}
            </Col>
        </Row>
    );
}

function compare(a: string, b: string): number {
    return a.toLocaleLowerCase().localeCompare(b.toLocaleLowerCase());
}
