interface PingResponse {
    message: string;
}

export async function getPing(): Promise<PingResponse> {
    const response = await fetch(
        "https://localhost:7078/api/ping"
    );

    return await response.json();
}