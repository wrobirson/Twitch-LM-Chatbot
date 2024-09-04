import {useQuery} from "@tanstack/react-query";
import {fetchBroadcasterAccount} from "@/api/broadcaster-account.ts";

export function useFetchBroadcasterAccount() {
    return useQuery({
        queryKey: ["broadcaster-account"],
        queryFn: () => fetchBroadcasterAccount()
    });
}