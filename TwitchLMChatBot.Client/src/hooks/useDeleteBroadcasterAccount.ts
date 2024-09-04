import {useMutation} from "@tanstack/react-query";
import {deleteBroadcasterAccount} from "@/api/broadcaster-account.ts";

export function useDeleteBroadcasterAccount() {
    return useMutation({
        mutationKey: ["broadcaster-account"],
        mutationFn: deleteBroadcasterAccount
    });
}