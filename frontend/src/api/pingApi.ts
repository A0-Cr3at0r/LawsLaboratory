import { API_URL } from "../config/api";

interface PingResponse {
    message: string;
}

export async function getPing(): Promise<PingResponse> {
    const response = await fetch(`${API_URL}/api/ping`);

    if (!response.ok) {
        throw new Error(`HTTP ${response.status}`);
    }

    return response.json();
}