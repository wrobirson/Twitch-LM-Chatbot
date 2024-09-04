import {useMutation} from "@tanstack/react-query";
import {setDefaultProvider} from "@/api/providers.ts";

export function useSetDefaultProvider() {
    return useMutation({
        mutationKey: ["set-default-provider"],
        mutationFn: setDefaultProvider,
    });
}