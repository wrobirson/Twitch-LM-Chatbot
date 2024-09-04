import {useMutation} from "@tanstack/react-query";
import {setDefaultPersonality} from "@/api/personalities.ts";

export function useSetDefaultPersonality() {
    return useMutation({
        mutationKey: ["set-default-personality"],
        mutationFn: setDefaultPersonality
    });
}