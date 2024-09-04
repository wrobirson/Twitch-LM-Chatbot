export type Personality ={
    "name": string,
    "model": string,
    "instructions": string
    "isDefault": boolean,
    "id": number
}

export type CreatePersonalityRequest  ={
    personalityName:string,
    instructions: string,
}

export type UpdatePersonalityRequest  ={
    personalityName:string,
    instructions: string,
}