import { useEffect, useState } from "react";
import { ticketsApi } from "../api/ticketsApi";
import type { Ticket } from "../types/ticket";
import { useNavigate } from "react-router-dom";
import CreateTicketModal from "../components/CreateTicketModal";
import { useAuth } from "../auth/AuthContext";

export function TicketsPage() {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [status, setStatus] = useState<string>("");
  const [search, setSearch] = useState<string>("");
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const { isAdmin, logout } = useAuth();

  const navigate = useNavigate();

  useEffect(() => {
    let isCancelled = false;

    async function fetchTickets() {
      try {
        const data = await ticketsApi.getAll({
          status: status || undefined,
          search: search || undefined,
        });

        if (!isCancelled) {
          setTickets(data);
        }
      } catch (err) {
        console.error(err);
      }
    }

    void fetchTickets();

    return () => {
      isCancelled = true;
    };
  }, [status, search]);

  return (
    <>
      <div className="p-8">
  <div className="flex justify-between items-center mb-6">
  <h1 className="text-3xl font-semibold">Support Tickets</h1>

  <div className="flex gap-3">

    {/* LOGIN / LOGOUT BUTTON */}
    <button
      onClick={() => {
        logout();
        navigate("/login");
      }}
      className={`px-4 py-2 rounded text-white ${
        isAdmin ? "bg-red-600 hover:bg-red-700" : "bg-gray-700 hover:bg-gray-800"
      }`}
    >
      {isAdmin ? "Logout" : "Login"}
    </button>

    {/* New Ticket Button */}
    <button
      onClick={() => setIsCreateModalOpen(true)}
      className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
    >
      + New Ticket
    </button>

  </div>
</div>


        {/* Filters */}
        <div className="flex gap-4 mb-6">
          <select
            value={status}
            onChange={(e) => setStatus(e.target.value)}
            className="border px-2 py-1 rounded"
          >
            <option value="">All</option>
            <option value="New">New</option>
            <option value="In Progress">In Progress</option>
            <option value="Closed">Closed</option>
            <option value="Resolved">Resolved</option>
          </select>

          <input
            type="text"
            placeholder="Search name or description"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="border px-2 py-1 rounded w-64"
          />
        </div>

        {/* Tickets Table */}
        <table className="w-full border-collapse">
          <thead>
            <tr className="bg-gray-100 text-left">
              <th className="p-2 border">Name</th>
              <th className="p-2 border">Email</th>
              <th className="p-2 border">Description</th>
              <th className="p-2 border">Status</th>
              <th className="p-2 border">Updated</th>
            </tr>
          </thead>

          <tbody>
            {tickets.map((t) => (
              <tr
                key={t.id}
                className="cursor-pointer hover:bg-gray-100"
                onClick={() => navigate(`/tickets/${t.id}`)}
              >
                <td className="p-2 border">{t.name}</td>
                <td className="p-2 border">{t.email}</td>
                <td className="p-2 border">{t.description}</td>
                <td className="p-2 border">{t.status}</td>
                <td className="p-2 border">
                  {new Date(t.updatedAt).toLocaleString()}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {isCreateModalOpen && (
        <CreateTicketModal
          onClose={() => setIsCreateModalOpen(false)}
          onCreated={(ticket: Ticket) =>
            setTickets((prev) => [ticket, ...prev])
          }
        />
      )}
    </>
  );
}

export default TicketsPage;
