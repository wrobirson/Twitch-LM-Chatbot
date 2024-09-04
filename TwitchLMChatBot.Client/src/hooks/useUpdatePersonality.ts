import {updatePersonality} from "@/api/personalities.ts";
import {useMutation} from "@tanstack/react-query";

export function useUpdatePersonality() {
    return useMutation({
        mutationKey: ['update-personality'],
        mutationFn: updatePersonality
    })
}
