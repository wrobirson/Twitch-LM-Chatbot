import { useQuery } from "@tanstack/react-query";
import { fetchPersonalities } from "@/api/personalities.ts";

export function useFetchPersonalities() {
    return useQuery({
        queryKey: ["personalities"],
        queryFn: () => fetchPersonalities(),
    });
}