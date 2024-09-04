import {useMutation} from "@tanstack/react-query";
import {createProvider} from "@/api/providers.ts";

export function useCreateProvider() {
    return useMutation({
        mutationKey: ["create-provider"],
        mutationFn: createProvider,
    });
}