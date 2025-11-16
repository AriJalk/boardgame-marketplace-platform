export interface UserSummary {
    id: string;
    displayName: string;
}

export interface UserLogin {
    id: string;
    role: RoleType
}

export enum RoleType {
    User = 1,
    Moderator = 2,
    Admin = 3
}