import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { ticketsApi } from "../api/ticketsApi";
import { isAdmin } from "../utils/auth";
import { ArrowLeft, Mail, User, Clock, Pencil } from "lucide-react";
import type { TicketStatus } from "../types/ticket";
import type { Ticket } from "../types/ticket";

export default function TicketDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const [ticket, setTicket] = useState<Ticket | null>(null);
  const [status, setStatus] = useState<TicketStatus>(ticket?.status ?? "New");
  const [resolution, setResolution] = useState("");
  
  useEffect(() => {
    if (!id) return;

      ticketsApi.getById(id).then((t) => {
        setTicket(t);
        setStatus(t.status);
        setResolution(t.resolution || "");
      });
  }, [id]);

  if (!ticket)
    return (
      <div className="p-8 text-center text-gray-500">
        Loading ticket...
      </div>
    );

  async function saveChanges() {
    if (!ticket) {
      alert("No ticket loaded");
      return;
    }
    await ticketsApi.update(ticket.id, { status, resolution });
    alert("Ticket updated");
  }

  // Status badge helper
  const statusColors: Record<string, string> = {
    "New": "bg-blue-100 text-blue-700",
    "In Progress": "bg-yellow-100 text-yellow-700",
    "Resolved": "bg-green-100 text-green-700",
    "Closed": "bg-gray-200 text-gray-700",
  };

  return (
    <div className="p-8 max-w-3xl mx-auto">

      {/* Back button */}
      <button
        onClick={() => navigate(-1)}
        className="flex items-center gap-2 text-gray-600 hover:text-gray-800 mb-6"
      >
        <ArrowLeft size={18} />
        Back
      </button>

      {/* Ticket Header */}
      <div className="bg-white shadow-sm rounded-xl p-6 border mb-6">
        <h1 className="text-2xl font-semibold mb-1">
          Ticket: {ticket.name}
        </h1>

        <div className="flex items-center gap-2">
          <span
            className={`inline-block px-3 py-1 text-sm rounded-full ${statusColors[ticket.status] || ""}`}
          >
            {status}
          </span>
        </div>
      </div>

      {/* Ticket Info */}
      <div className="bg-white shadow-sm rounded-xl p-6 border space-y-4">

        <div className="flex items-center gap-2 text-gray-700">
          <User size={18} /> <span className="font-medium">{ticket.name}</span>
        </div>

        <div className="flex items-center gap-2 text-gray-700">
          <Mail size={18} /> <span>{ticket.email}</span>
        </div>

        <div className="flex items-center gap-2 text-gray-700">
          <Clock size={18} /> 
          <span>
            Updated: {new Date(ticket.updatedAt).toLocaleString()}
          </span>
        </div>

        <div>
          <h3 className="font-semibold mb-1">Description:</h3>
          <p className="text-gray-800 whitespace-pre-wrap">{ticket.description}</p>
        </div>

        {ticket.summary && (
          <div>
            <h3 className="font-semibold mb-1">AI Summary:</h3>
            <p className="text-gray-700 italic border-l-4 border-blue-300 pl-3">
              {ticket.summary}
            </p>
          </div>
        )}

        <div>
          <h3 className="font-semibold mb-1">Resolution:</h3>
          <p className="text-gray-800 whitespace-pre-wrap">
            {resolution}
          </p>
        </div>

      </div>

      {/* Admin Edit Panel */}
      {isAdmin() && (
        <div className="mt-8 bg-gray-50 border rounded-xl p-6 shadow-sm">
          <h2 className="text-xl font-semibold mb-4 flex items-center gap-2">
            <Pencil size={20} /> Ticket Management
          </h2>

          <div className="mb-4">
            <label className="block text-sm font-medium mb-1">
              Status
            </label>
            <select
              value={status}
              onChange={(e) => setStatus(e.target.value as TicketStatus)}
              className="border rounded-lg px-3 py-2 w-48"
            >
              <option value="New">New</option>
              <option value="In Progress">In Progress</option>
              <option value="Resolved">Resolved</option>
              <option value="Closed">Closed</option>
            </select>
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium mb-1">
              Resolution
            </label>
            <textarea
              value={resolution}
              onChange={(e) => setResolution(e.target.value)}
              className="border rounded-lg px-3 py-2 w-full h-24 resize-none"
            />
          </div>

          <button
            onClick={saveChanges}
            className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
          >
            Save Changes
          </button>
        </div>
      )}
    </div>
  );
}
