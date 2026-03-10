/**
 * Camada de comunicação com a API.
 *
 * Centraliza todas as chamadas HTTP em um único lugar, utilizando axios.
 * A baseURL é configurada via variável de ambiente VITE_API_URL,
 * com fallback para http://localhost:5000/api em desenvolvimento.
 *
 * Cada função retorna diretamente o dado da resposta (r.data),
 * deixando as páginas livres de lidar com o objeto de resposta do axios.
 */
import axios from "axios";
import type {
  Category,
  CategorySummary,
  Person,
  Summary,
  Transaction,
} from "../types";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? "http://localhost:5000/api",
});

// ─── People ───────────────────────────────────────────────────────────────────

export const getPeople = () =>
  api.get<Person[]>("/people").then((r) => r.data);

export const createPerson = (data: { name: string; age: number }) =>
  api.post<Person>("/people", data).then((r) => r.data);

export const updatePerson = (
  id: string,
  data: { name: string; age: number }
) => api.put<Person>(`/people/${id}`, data).then((r) => r.data);

export const deletePerson = (id: string) => api.delete(`/people/${id}`);

// ─── Categories ───────────────────────────────────────────────────────────────

export const getCategories = () =>
  api.get<Category[]>("/categories").then((r) => r.data);

export const createCategory = (data: {
  description: string;
  purpose: number;
}) => api.post<Category>("/categories", data).then((r) => r.data);

// ─── Transactions ─────────────────────────────────────────────────────────────

export const getTransactions = () =>
  api.get<Transaction[]>("/transactions").then((r) => r.data);

export const createTransaction = (data: {
  description: string;
  value: number;
  type: number;
  categoryId: string;
  personId: string;
}) => api.post<Transaction>("/transactions", data).then((r) => r.data);

// ─── Summary ──────────────────────────────────────────────────────────────────

export const getSummaryByPerson = () =>
  api.get<Summary>("/summary/by-person").then((r) => r.data);

export const getSummaryByCategory = () =>
  api.get<CategorySummary>("/summary/by-category").then((r) => r.data);