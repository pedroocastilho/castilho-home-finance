/**
 * PeoplePage — gerenciamento de pessoas cadastradas no sistema.
 *
 * Responsabilidades desta página:
 * - Listar todas as pessoas via GET /api/people
 * - Criar novas pessoas via POST /api/people
 * - Editar uma pessoa existente via PUT /api/people/:id
 * - Excluir uma pessoa via DELETE /api/people/:id
 *
 * Ao excluir, o backend remove automaticamente todas as transações
 * vinculadas à pessoa (cascade delete configurado no EF Core).
 *
 * Pessoas com idade menor de 18 anos são identificadas visualmente
 * com um aviso laranja, sinalizando que só podem registrar despesas.
 */
import { useEffect, useState } from "react";
import {
  createPerson,
  deletePerson,
  getPeople,
  updatePerson,
} from "../services/api";
import type { Person } from "../types";

export default function PeoplePage() {
  const [people, setPeople] = useState<Person[]>([]);
  const [form, setForm] = useState({ name: "", age: "" });
  const [editing, setEditing] = useState<Person | null>(null);
  const [error, setError] = useState("");

  const load = () => getPeople().then(setPeople);

  useEffect(() => {
    load();
  }, []);

  const handleSubmit = async () => {
    setError("");
    try {
      const payload = { name: form.name, age: Number(form.age) };
      if (editing) {
        await updatePerson(editing.id, payload);
        setEditing(null);
      } else {
        await createPerson(payload);
      }
      setForm({ name: "", age: "" });
      load();
    } catch (e: any) {
      setError(e.response?.data ?? "Erro ao salvar.");
    }
  };

  const handleEdit = (person: Person) => {
    setEditing(person);
    setForm({ name: person.name, age: String(person.age) });
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Excluir pessoa e todas as suas transações?")) return;
    await deletePerson(id);
    load();
  };

  return (
    <div className="p-6 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Pessoas</h1>

      {/* Formulário */}
      <div className="bg-white rounded-xl shadow p-4 mb-6 space-y-3">
        <h2 className="font-semibold text-gray-700">
          {editing ? "Editar Pessoa" : "Nova Pessoa"}
        </h2>
        {error && <p className="text-red-500 text-sm">{error}</p>}
        <input
          className="border rounded px-3 py-2 w-full"
          placeholder="Nome"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          maxLength={200}
        />
        <input
          className="border rounded px-3 py-2 w-full"
          placeholder="Idade"
          type="number"
          value={form.age}
          onChange={(e) => setForm({ ...form, age: e.target.value })}
        />
        <div className="flex gap-2">
          <button
            onClick={handleSubmit}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            {editing ? "Salvar" : "Adicionar"}
          </button>
          {editing && (
            <button
              onClick={() => {
                setEditing(null);
                setForm({ name: "", age: "" });
              }}
              className="px-4 py-2 rounded border hover:bg-gray-50"
            >
              Cancelar
            </button>
          )}
        </div>
      </div>

      {/* Lista */}
      <div className="space-y-2">
        {people.map((p) => (
          <div
            key={p.id}
            className="bg-white rounded-xl shadow px-4 py-3 flex justify-between items-center"
          >
            <div>
              <span className="font-medium">{p.name}</span>
              <span className="text-gray-500 text-sm ml-2">
                {p.age} anos
                {p.isMinor && (
                  <span className="ml-2 text-orange-500 text-xs font-semibold">
                    (menor de idade)
                  </span>
                )}
              </span>
            </div>
            <div className="flex gap-2">
              <button
                onClick={() => handleEdit(p)}
                className="text-blue-600 text-sm hover:underline"
              >
                Editar
              </button>
              <button
                onClick={() => handleDelete(p.id)}
                className="text-red-500 text-sm hover:underline"
              >
                Excluir
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}