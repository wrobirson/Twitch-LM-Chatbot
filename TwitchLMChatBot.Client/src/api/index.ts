import {Api} from "@/api/generated.ts";
import env from "@/env.ts";

const apiClient = new Api({
    baseUrl: env.API_BASE_URL
})

export default apiClient;