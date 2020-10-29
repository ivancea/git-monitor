export type ChangeWrapper = {
  repository: string;
  date: Date;
  change: Change;
  seen: boolean;
};

export type Changes = Record<string, Change[]>;

export type Change = {
  objectType: ChangeObjectType;
  type: ChangeType;
  objectName: string;
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
  tagger?: ChangeUser;
};

export function isTagChange(change: Change): change is TagChange {
  return change.objectType === ChangeObjectType.Tag;
}
