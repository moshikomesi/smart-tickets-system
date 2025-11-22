import { httpGet, httpPost, httpPut } from "./httpClient";
import type { 
  Ticket, 
  CreateTicketRequest, 
  UpdateTicketRequest 
} from "../types/ticket";

export interface GetTicketsParams {
  status?: string;
  search?: string;
}

export const ticketsApi = {
getAll: (params?: GetTicketsParams) => {
    const query = new URLSearchParams();

    if (params?.status) {
      query.append("status", params.status);
    }

    if (params?.search) {
      query.append("search", params.search);
    }

    const qs = query.toString();
    const path = qs ? `/api/tickets?${qs}` : "/api/tickets";

    return httpGet<Ticket[]>(path);
  },
  getById: (id: string) => httpGet<Ticket>(`/api/tickets/${id}`),

create: (body: CreateTicketRequest) =>
  httpPost<Ticket, CreateTicketRequest>("/api/tickets", body),

  update: (id: string, body: UpdateTicketRequest) =>
    httpPut<Ticket, UpdateTicketRequest>(`/api/tickets/${id}`, body),
};
