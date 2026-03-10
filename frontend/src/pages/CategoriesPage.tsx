/**
 * CategoriesPage — gerenciamento de categorias de transações.
 *
 * Responsabilidades desta página:
 * - Listar todas as categorias via GET /api/categories
 * - Criar novas categorias via POST /api/categories
 *
 * Cada categoria possui uma finalidade (Despesa, Receita ou Ambas),
 * que determina em quais tipos de transação ela pode ser utilizada.
 * Essa restrição é aplicada tanto no frontend (filtrando as opções
 * disponíveis no formulário de transação) quanto no backend.
 */
import { useEffect, useState } from "react";
import { createCategory, getCategories } from "../services/api";
import type { Category } from "../types";
import { CategoryPurpose } from "../types";

const PURPOSE_LABELS: Record<CategoryPurpose, string> = {
  [CategoryPurpose.Expense]: "Despesa",
  [CategoryPurpose.Income]: "Receita",
  [CategoryPurpose.Both]: "Ambas",
};

export default function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [form, setForm] = useState({ description: "", purpose: CategoryPurpose.Both });
  const [error, setError] = useState("");

  const load = () => getCategories().then(setCategories);

  useEffect(() => {
    load();
  }, []);

  const handleSubmit = async () => {
    setError("");
    try {
      await createCategory({ description: form.description, purpose: form.purpose });
      setForm({ description: "", purpose: CategoryPurpose.Both });
      load();
    } catch (e: any) {
      setError(e.response?.data ?? "Erro ao salvar.");
    }
  };

  const purposeColor: Record<CategoryPurpose, string> = {
    [CategoryPurpose.Expense]: "bg-red-100 text-red-700",
    [CategoryPurpose.Income]: "bg-green-100 text-green-700",
    [CategoryPurpose.Both]: "bg-blue-100 text-blue-700",
  };

  return (
    <div className="p-6 max-w-3xl mx-auto">
      <h1 className="text-2xl font-bold mb-6">Categorias</h1>

      <div className="bg-white rounded-xl shadow p-4 mb-6 space-y-3">
        <h2 className="font-semibold text-gray-700">Nova Categoria</h2>
        {error && <p className="text-red-500 text-sm">{error}</p>}
        <input
          className="border rounded px-3 py-2 w-full"
          placeholder="Descrição"
          value={form.description}
          onChange={(e) => setForm({ ...form, description: e.target.value })}
          maxLength={400}
        />
        <select
          className="border rounded px-3 py-2 w-full"
          value={form.purpose}
          onChange={(e) => setForm({ ...form, purpose: Number(e.target.value) as CategoryPurpose })}
        >
          <option value={CategoryPurpose.Expense}>Despesa</option>
          <option value={CategoryPurpose.Income}>Receita</option>
          <option value={CategoryPurpose.Both}>Ambas</option>
        </select>
        <button
          onClick={handleSubmit}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Adicionar
        </button>
      </div>

      <div className="space-y-2">
        {categories.map((c) => (
          <div key={c.id} className="bg-white rounded-xl shadow px-4 py-3 flex justify-between items-center">
            <span className="font-medium">{c.description}</span>
            <span className={`text-xs font-semibold px-2 py-1 rounded-full ${purposeColor[c.purpose]}`}>
              {PURPOSE_LABELS[c.purpose]}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
}