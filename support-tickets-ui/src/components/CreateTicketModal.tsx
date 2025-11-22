import { useState } from "react";
import { ticketsApi } from "../api/ticketsApi";
import type { Ticket } from "../types/ticket";

interface CreateTicketModalProps {
  onClose: () => void;
  onCreated: (ticket: Ticket) => void;
}

export default function CreateTicketModal({ onClose, onCreated }: CreateTicketModalProps) {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [description, setDescription] = useState("");

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    try {
      const newTicket = await ticketsApi.create({
        name,
        email,
        description
      });

      onCreated(newTicket);
      onClose();
    } catch (err) {
      alert("Failed to create ticket");
      console.error(err);
    }
  }

  return (
    <div className="fixed inset-0 bg-black/40 flex items-center justify-center animate-fadeIn">
      <div className="bg-white p-6 rounded-lg shadow-xl w-96 animate-scaleIn">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">Create New Ticket</h2>
          <button onClick={onClose} className="text-gray-500 hover:text-black">✕</button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block mb-1 text-sm">Full Name</label>
            <input
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="border rounded px-2 py-1 w-full"
            />
          </div>

          <div>
            <label className="block mb-1 text-sm">Email</label>
            <input
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="border rounded px-2 py-1 w-full"
            />
          </div>

          <div>
            <label className="block mb-1 text-sm">Description</label>
            <textarea
              required
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="border rounded px-2 py-1 w-full"
            />
          </div>

          <div className="flex justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
            >
              Cancel
            </button>

            <button
              type="submit"
              className="px-3 py-1 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              Create
            </button>
          </div>

        </form>
      </div>
    </div>
  );
}
