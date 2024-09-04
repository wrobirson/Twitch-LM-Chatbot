import axios from "@/api/axios.ts";
import {CreatePersonalityRequest, Personality, UpdatePersonalityRequest} from "@/api/types/personalities.ts";

const ENDPOINT = "/api/personalities"

export const fetchPersonalities = async () => {
    const response = await axios.get<Personality[]>(ENDPOINT)
    return response.data
}

export const createPersonality = async (data: CreatePersonalityRequest) => {
    const response = await axios.post(ENDPOINT, data)
    return response.data
}

export const setDefaultPersonality = async (id: number) => {
    const response = await axios.put(`${ENDPOINT}/${id}/default`)
    return response.data
}

export const updatePersonality = async ({id, data}: { id: number, data: UpdatePersonalityRequest }) => {
    const response = await axios.put(`${ENDPOINT}/${id}`, data)
    return response.data
}

export const deletePersonality = async (id: number) => {
    const response = await axios.delete(`${ENDPOINT}/${id}`)
    return response.data
}