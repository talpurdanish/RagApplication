// helpers/ApiUtils.ts
export function normalizePayload(payload: any): {
    params?: Record<string, any>;
    queryString?: string;
} {
    if (payload == null) return {};

    // If payload is already a plain object → use as params
    if (typeof payload === "object" && !Array.isArray(payload)) {
        return { params: payload };
    }

    // If payload is a string or number → treat as query string
    return { queryString: String(payload) };
}
