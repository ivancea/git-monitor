import React, { Dispatch, SetStateAction, useCallback } from "react";
import { useId } from "react-id-generator";
import { Button, Col, Container, Row, UncontrolledCollapse } from "reactstrap";
import { Filter } from "../../../hooks/useChanges";
import { ChangeObjectType, ChangeType, ChangeWrapper } from "../../../types/changes";
import { ChangeFilterRegexInput } from "./ChangeFilterRegexInput";
import { ChangeFilterSelector } from "./ChangeFilterSelector";

type Props = {
    changes: ChangeWrapper[];
    filter: Filter;
    setFilter: Dispatch<SetStateAction<Filter>>;
};

const changeTypes = Object.keys(ChangeType).filter((t) => typeof t === "string");
const changeObjectTypes = Object.keys(ChangeObjectType).filter((t) => typeof t === "string");

export function ChangesFilters({ changes, setFilter }: Props): React.ReactElement {
    const [filtersTogglerId] = useId();

    const useFilterChanged = (
        filterType: keyof Filter,
    ): ((elements: Map<string | undefined, boolean> | RegExp) => void) =>
        useCallback(
            (filter: Map<string | undefined, boolean> | RegExp) =>
                setFilter((f) => ({
                    ...f,
                    [filterType]: filter,
                })),
            [filterType],
        );

    const onRepositoriesFilterChanged = useFilterChanged("repositories");
    const onUsersFilterChanged = useFilterChanged("users");
    const onTypesFilterChanged = useFilterChanged("types");
    const onObjectTypesFilterChanged = useFilterChanged("objectTypes");

    const onBranchFilterChanged = useFilterChanged("branchRegex");
    const onTagFilterChanged = useFilterChanged("tagRegex");
    const onCommitFilterChanged = useFilterChanged("commitRegex");

    return (
        <Container>
            <Row>
                <Col xs="1">
                    <Button id={filtersTogglerId} outline color="info">
                        Filters
                    </Button>
                </Col>
                <Col>
                    <UncontrolledCollapse
                        defaultOpen={true}
                        toggler={`#${filtersTogglerId}`}
                        style={{
                            borderLeft: "1px solid black",
                        }}
                    >
                        <ChangeFilterSelector
                            name="Repositories"
                            changes={changes}
                            accessor={useCallback((c) => c.repository, [])}
                            onChanged={onRepositoriesFilterChanged}
                        />
                        <ChangeFilterSelector
                            name="Users"
                            changes={changes}
                            accessor={useCallback((c) => (c.change.user ? c.change.user.name : ""), [])}
                            textSelector={useCallback((o: string) => (o === "" ? <i>No user</i> : o), [])}
                            onChanged={onUsersFilterChanged}
                        />
                        <ChangeFilterSelector
                            name="Types"
                            changes={changes}
                            defaultOptions={changeTypes}
                            accessor={useCallback((c) => c.change.type, [])}
                            onChanged={onTypesFilterChanged}
                        />
                        <ChangeFilterSelector
                            name="Object types"
                            changes={changes}
                            defaultOptions={changeObjectTypes}
                            accessor={useCallback((c) => c.change.objectType, [])}
                            onChanged={onObjectTypesFilterChanged}
                        />
                        <ChangeFilterRegexInput name="Branch regex" onChanged={onBranchFilterChanged} />
                        <ChangeFilterRegexInput name="Tag regex" onChanged={onTagFilterChanged} />
                        <ChangeFilterRegexInput name="Commit regex" onChanged={onCommitFilterChanged} />
                    </UncontrolledCollapse>
                </Col>
            </Row>
        </Container>
    );
}
