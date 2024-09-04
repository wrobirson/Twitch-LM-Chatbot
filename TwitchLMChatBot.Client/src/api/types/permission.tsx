export type PermissionSet = {
    unrestricted: boolean,
    followers: boolean,
    subscribers: boolean,
    moderators: boolean,
    vips: boolean,
    whiteList: string[],
    blackList: string[],
}