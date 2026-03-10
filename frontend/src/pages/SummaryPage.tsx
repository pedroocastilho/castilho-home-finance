/**
 * SummaryPage — página inicial com os totalizadores financeiros.
 *
 * Exibe dois painéis:
 * 1. Totais por pessoa: receitas, despesas e saldo individual de cada
 *    pessoa cadastrada, seguido de um total geral consolidado.
 * 2. Totais por categoria: mesma estrutura, agrupada por categoria.
 *
 * Os dados são calculados no backend (SummaryService) e retornados
 * já totalizados, sem processamento adicional no frontend.
 */
import { useEffect, useState } from "react";
import { getSummaryByCategory, getSummaryByPerson } from "../services/api";
import type { CategorySummary, Summary } from "../types";

const fmt = (v: number) =>
  v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });

const BalanceCell = ({ value }: { value: number }) => (
  <td className={`px-4 py-3 text-right font-semibold ${value >= 0 ? "text-green-600" : "text-red-500"}`}>
    {fmt(value)}
  </td>
);

export default function SummaryPage() {
  const [personSummary, setPersonSummary] = useState<Summary | null>(null);
  const [categorySummary, setCategorySummary] = useState<CategorySummary | null>(null);

  useEffect(() => {
    getSummaryByPerson().then(setPersonSummary);
    getSummaryByCategory().then(setCategorySummary);
  }, []);

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-10">
      <h1 className="text-2xl font-bold">Resumo Financeiro</h1>

      {/* Por Pessoa */}
      <section>
        <h2 className="text-lg font-semibold mb-3">Por Pessoa</h2>
        <div className="bg-white rounded-xl shadow overflow-hidden">
          <table className="w-full text-sm">
            <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
              <tr>
                <th className="px-4 py-3 text-left">Pessoa</th>
                <th className="px-4 py-3 text-right">Receitas</th>
                <th className="px-4 py-3 text-right">Despesas</th>
                <th className="px-4 py-3 text-right">Saldo</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {personSummary?.byPerson.map((row) => (
                <tr key={row.personId}>
                  <td className="px-4 py-3">{row.personName}</td>
                  <td className="px-4 py-3 text-right text-green-600">{fmt(row.totalIncome)}</td>
                  <td className="px-4 py-3 text-right text-red-500">{fmt(row.totalExpense)}</td>
                  <BalanceCell value={row.balance} />
                </tr>
              ))}
            </tbody>
            {personSummary && (
              <tfoot className="bg-gray-50 font-semibold text-gray-700">
                <tr>
                  <td className="px-4 py-3">Total Geral</td>
                  <td className="px-4 py-3 text-right text-green-600">{fmt(personSummary.grandTotalIncome)}</td>
                  <td className="px-4 py-3 text-right text-red-500">{fmt(personSummary.grandTotalExpense)}</td>
                  <BalanceCell value={personSummary.grandBalance} />
                </tr>
              </tfoot>
            )}
          </table>
        </div>
      </section>

      {/* Por Categoria */}
      <section>
        <h2 className="text-lg font-semibold mb-3">Por Categoria</h2>
        <div className="bg-white rounded-xl shadow overflow-hidden">
          <table className="w-full text-sm">
            <thead className="bg-gray-50 text-gray-600 uppercase text-xs">
              <tr>
                <th className="px-4 py-3 text-left">Categoria</th>
                <th className="px-4 py-3 text-right">Receitas</th>
                <th className="px-4 py-3 text-right">Despesas</th>
                <th className="px-4 py-3 text-right">Saldo</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {categorySummary?.byCategory.map((row) => (
                <tr key={row.categoryId}>
                  <td className="px-4 py-3">{row.categoryDescription}</td>
                  <td className="px-4 py-3 text-right text-green-600">{fmt(row.totalIncome)}</td>
                  <td className="px-4 py-3 text-right text-red-500">{fmt(row.totalExpense)}</td>
                  <BalanceCell value={row.balance} />
                </tr>
              ))}
            </tbody>
            {categorySummary && (
              <tfoot className="bg-gray-50 font-semibold text-gray-700">
                <tr>
                  <td className="px-4 py-3">Total Geral</td>
                  <td className="px-4 py-3 text-right text-green-600">{fmt(categorySummary.grandTotalIncome)}</td>
                  <td className="px-4 py-3 text-right text-red-500">{fmt(categorySummary.grandTotalExpense)}</td>
                  <BalanceCell value={categorySummary.grandBalance} />
                </tr>
              </tfoot>
            )}
          </table>
        </div>
      </section>
    </div>
  );
}