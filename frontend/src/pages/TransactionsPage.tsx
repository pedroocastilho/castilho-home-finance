/**
 * TransactionsPage — registro e listagem de transações financeiras.
 *
 * Responsabilidades desta página:
 * - Listar todas as transações via GET /api/transactions
 * - Criar novas transações via POST /api/transactions
 *
 * Regras de negócio aplicadas no frontend:
 * - Ao selecionar o tipo da transação (despesa/receita), o select de
 *   categorias é filtrado automaticamente para exibir apenas as categorias
 *   com finalidade compatível. Isso evita que o usuário selecione uma
 *   combinação inválida antes mesmo de enviar para a API.
 * - Pessoas menores de 18 anos são identificadas no select com um aviso,
 *   sinalizando que só aceitam transações do tipo despesa.
 *
 * Todas as regras são validadas novamente no backend antes de persistir.
 */
import { useEffect, useState } from "react";
import {
  createTransaction,
  getCategories,
  getPeople,
  getTransactions,
} from "../services/api";
import type { Category, Person, Transaction } from "../types";
import { CategoryPurpose, TransactionType } from "../types";

export default function TransactionsPage() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [people, setPeople] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [form, setForm] = useState({
    description: "",
    value: "",
    type: TransactionType.Expense,
    categoryId: "",
    personId: "",
  });
  const [error, setError] = useState("");

  const load = () => {
    getTransactions().then(setTransactions);
    getPeople().then(setPeople);
    getCategories().then(setCategories);
  };

  useEffect(() => { load(); }, []);

  // Filtra categorias compatíveis com o tipo da transação selecionada
  const compatibleCategories = categories.filter((c) => {
    if (form.type === TransactionType.Expense)
      return c.purpose === CategoryPurpose.Expense || c.purpose === CategoryPurpose.Both;
    return c.purpose === CategoryPurpose.Income || c.purpose === CategoryPurpose.Both;
  });

  // Quando o tipo muda, limpa a categoria (pode ter ficado incompatível)
  const handleTypeChange = (type: TransactionType) => {
    setForm({ ...form, type, categoryId: "" });
  };

  const handleSubmit = async () => {
    setError("");
    try {
      await createTransaction({
        description: form.description,
        value: Number(form.value),
        type: form.type,
        categoryId: form.categoryId,
        personId: form.personId,
      });
      setForm({ description: "", value: "", type: TransactionType.Expense, categoryId: "", personId: "" });
      getTransactions().then(setTransactions);
    } catch (e: any) {
      setError(e.response?.data ?? "Erro ao salvar.");
    }
  };

  const fmt = (v: number) =>
    v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Transações</h1>

      <div className="bg-white rounded-xl shadow p-4 mb-6 space-y-3">
        <h2 className="font-semibold text-gray-700">Nova Transação</h2>
        {error && <p className="text-red-500 text-sm">{error}</p>}

        <input
          className="border rounded px-3 py-2 w-full"
          placeholder="Descrição"
          value={form.description}
          onChange={(e) => setForm({ ...form, description: e.target.value })}
          maxLength={400}
        />

        <div className="grid grid-cols-2 gap-3">
          <input
            className="border rounded px-3 py-2"
            placeholder="Valor"
            type="number"
            min="0.01"
            step="0.01"
            value={form.value}
            onChange={(e) => setForm({ ...form, value: e.target.value })}
          />
          <select
            className="border rounded px-3 py-2"
            value={form.type}
            onChange={(e) => handleTypeChange(Number(e.target.value) as TransactionType)}
          >
            <option value={TransactionType.Expense}>Despesa</option>
            <option value={TransactionType.Income}>Receita</option>
          </select>
        </div>

        <select
          className="border rounded px-3 py-2 w-full"
          value={form.personId}
          onChange={(e) => setForm({ ...form, personId: e.target.value })}
        >
          <option value="">Selecione a pessoa</option>
          {people.map((p) => (
            <option key={p.id} value={p.id}>
              {p.name} {p.isMinor ? "(menor - só despesas)" : ""}
            </option>
          ))}
        </select>

        <select
          className="border rounded px-3 py-2 w-full"
          value={form.categoryId}
          onChange={(e) => setForm({ ...form, categoryId: e.target.value })}
        >
          <option value="">Selecione a categoria</option>
          {compatibleCategories.map((c) => (
            <option key={c.id} value={c.id}>{c.description}</option>
          ))}
        </select>

        <button
          onClick={handleSubmit}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Registrar
        </button>
      </div>

      <div className="space-y-2">
        {transactions.map((t) => (
          <div key={t.id} className="bg-white rounded-xl shadow px-4 py-3">
            <div className="flex justify-between items-start">
              <div>
                <p className="font-medium">{t.description}</p>
                <p className="text-sm text-gray-500">
                  {t.person.name} · {t.category.description}
                </p>
              </div>
              <span className={`font-bold ${t.type === TransactionType.Income ? "text-green-600" : "text-red-500"}`}>
                {t.type === TransactionType.Income ? "+" : "-"}{fmt(t.value)}
              </span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}