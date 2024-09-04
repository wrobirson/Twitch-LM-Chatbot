import {useQuery} from "@tanstack/react-query";
import {fetchProviders} from "@/api/providers.ts";

export function useFetchProviders() {
    return useQuery({
        queryKey: ["providers"],
        queryFn: () => fetchProviders()
    });
}