import axios from "@/api/axios.ts";
import {User} from "@/api/types/broadcaster-account.ts";

const ENDPOINT = "/api/broadcaster-account"

export const fetchBroadcasterAccount = async () => {
    const response = await axios.get<User>(ENDPOINT)
    return response.data
};

export const deleteBroadcasterAccount = async () => {
    const response = await  axios.delete(ENDPOINT)
    return response.data
};