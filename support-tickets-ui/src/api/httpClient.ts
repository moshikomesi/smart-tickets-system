const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5079";
  
function getAuthHeaders(): Record<string, string> {
const token = localStorage.getItem("token");
return token ? { Authorization: `Bearer ${token}` } : {};
}


async function handleResponse<T>(res: Response, path: string): Promise<T> {
if (!res.ok) {
const text = await res.text().catch(() => "");
throw new Error(`HTTP ${res.status} on ${path}: ${text}`);
}


// No content
if (res.status === 204) return {} as T;


return res.json() as Promise<T>;
}


export async function httpGet<T>(path: string): Promise<T> {
const headers: Record<string, string> = {
"Content-Type": "application/json",
...getAuthHeaders(),
};


const res = await fetch(`${API_BASE_URL}${path}`, {
method: "GET",
headers,
});


return handleResponse<T>(res, path);
}


export async function httpPost<TicketResponse, CreateTicketRequest>(
  path: string,
  body: CreateTicketRequest
): Promise<TicketResponse> {
  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...getAuthHeaders(),
  };

  const res = await fetch(`${API_BASE_URL}${path}`, {
    method: "POST",
    headers,
    body: JSON.stringify(body),
  });

  return handleResponse<TicketResponse>(res, path);
}


export async function httpPut<TicketResponse, UpdateTicketRequest>(
  path: string,
  body: UpdateTicketRequest
): Promise<TicketResponse> {
  const headers: Record<string, string> = {
    "Content-Type": "application/json",
...getAuthHeaders(),
};


const res = await fetch(`${API_BASE_URL}${path}`, {
method: "PUT",
headers,
body: JSON.stringify(body),
});


return handleResponse<TicketResponse>(res, path);
}
