import axios from "@/api/axios.ts";
import {CreateProviderRequest, Provider} from "@/api/types/provider.ts";


const ENDPOINT = "/api/providers"

export const fetchProviders = async () => {
    const  response = await axios.get<Provider[]>(ENDPOINT)
    return response.data
}

export const createProvider = async (provider: CreateProviderRequest) => {
    const response = await axios.post(ENDPOINT, provider)
    return response.data
}

export const deleteProvider = async (providerId: number) => {
    const response = await axios.delete(`${ENDPOINT}/${providerId}`)
    return response.data
}

export const setDefaultProvider= async (id: number) => {
    const  response = await axios.put(`${ENDPOINT}/${id}/default`)
    return response.data
}