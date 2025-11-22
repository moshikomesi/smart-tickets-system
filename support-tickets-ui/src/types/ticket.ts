export type TicketStatus = 
  | "New" 
  | "In Progress" 
  | "Closed" 
  | "Resolved";

export interface Ticket {
  id: string;
  name: string;
  email: string;
  description: string;
  summary?: string;
  imageUrl?: string;
  status: TicketStatus;
  resolution?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateTicketRequest {
  name: string;
  email: string;
  description: string;
}

export interface UpdateTicketRequest {
  status?: TicketStatus;
  resolution?: string;
}
export interface TicketResponse { 
  id: string;
  name: string;
  email: string;
  description: string;
  status: string;
  updatedAt: string;
}