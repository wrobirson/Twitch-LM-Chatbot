import myAxios from "@/api/axios.ts";
import {PermissionSet} from "@/api/types/permission.tsx";

export async function fetchPermissions() {
    const response = await myAxios.get<PermissionSet>("/api/permissions");
    return response.data
}