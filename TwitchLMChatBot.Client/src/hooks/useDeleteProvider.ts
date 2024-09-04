import {useMutation} from "@tanstack/react-query";
import {deleteProvider} from "@/api/providers.ts";

export function useDeleteProvider() {
    return useMutation({
        mutationKey: ["delete-provider"],
        mutationFn: deleteProvider,
    });
}

