import {useMutation, useQuery} from "@tanstack/react-query";
import {fetchPermissions} from "@/api/permissions.tsx";
import myAxios from "@/api/axios.ts";
import {PermissionSet} from "@/api/types/permission.tsx";

export function useFetchPermissions() {
    return useQuery({
        queryKey: ['permission'],
        queryFn: async () => await fetchPermissions()
    })
}

async function setPermission(data: PermissionSet) {
   return await myAxios.put("/api/permissions", data)
}

export function useSetPermissions() {
    return useMutation({
        mutationKey: ['permission'],
        mutationFn: setPermission
    })
}