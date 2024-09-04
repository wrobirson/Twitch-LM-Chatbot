import {useMutation} from "@tanstack/react-query";
import {createPersonality} from "@/api/personalities.ts";

export function useCreatePersonality(){
    return useMutation({
        mutationKey: ['createPersonality'],
        mutationFn: createPersonality
    })
}