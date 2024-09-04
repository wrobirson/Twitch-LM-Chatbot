export type CreateProviderRequest = {
    name: string;
    providerType: 0|1;
    apiKey?: string;
    baseUrl?: string;
    model?: string;
}

export type Provider = {
    "name": string,
    "type": 0|1,
    "baseUrl": string,
    "apiKey": string,
    "isDefault": boolean,
    "typeName": string,
    "id": number
}
