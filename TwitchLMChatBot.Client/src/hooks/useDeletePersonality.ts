import {useMutation} from "@tanstack/react-query";
import {deletePersonality} from "@/api/personalities.ts";

export function useDeletePersonality() {
    return useMutation({
        mutationKey: ["delete-personality"],
        mutationFn: deletePersonality,
    });
}