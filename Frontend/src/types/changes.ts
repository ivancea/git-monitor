export type ChangeWrapper<T extends Change = Change> = {
    repository: string;
    date: Date;
    change: T;
    seen: boolean;
};

export type Changes = Record<string, Change[]>;

export type Change = {
    objectType: ChangeObjectType;
    type: ChangeType;
    objectName: string;
    user?: ChangeUser;
};

export enum ChangeObjectType {
    Branch = "Branch",
    Tag = "Tag",
    Commit = "Commit",
}

export enum ChangeType {
    Created = "Created",
    Updated = "Updated",
    Deleted = "Deleted",
}

export type ChangeUser = {
    name: string;
    email: string;
};

export type BranchChange = Change & {
    objectType: ChangeObjectType.Branch;
    targetCommit: string;
};

export function isBranchChange(change: Change): change is BranchChange {
    return change.objectType === ChangeObjectType.Branch;
}

export type TagChange = Change & {
    objectType: ChangeObjectType.Tag;
    targetCommit: string;
    message?: string;
};

export function isTagChange(change: Change): change is TagChange {
    return change.objectType === ChangeObjectType.Tag;
}

export type CommitChange = Change & {
    objectType: ChangeObjectType.Commit;
    user: ChangeUser;
    hash: string;
    message: string;
};

export function isCommitChange(change: Change): change is CommitChange {
    return change.objectType === ChangeObjectType.Commit;
}
