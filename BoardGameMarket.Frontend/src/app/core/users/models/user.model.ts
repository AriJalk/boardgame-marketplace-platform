export interface UserSummary {
    id: string;
    displayName: string;
}

export interface UserLogin {
    id: string;
    displayName: string;
    role: RoleType;
    token: string;
}

export enum RoleType {
    User = 1,
    Moderator = 2,
    Admin = 3
}