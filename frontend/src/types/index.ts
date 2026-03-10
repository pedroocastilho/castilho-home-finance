/**
 * Tipagem TypeScript do frontend.
 *
 * Os enums e interfaces aqui refletem os modelos e enums do backend,
 * garantindo consistência entre as duas camadas sem depender de
 * geração automática de código.
 */
export enum TransactionType {
  Expense = 1,
  Income = 2,
}

export enum CategoryPurpose {
  Expense = 1,
  Income = 2,
  Both = 3,
}

export interface Person {
  id: string;
  name: string;
  age: number;
  isMinor: boolean;
}

export interface Category {
  id: string;
  description: string;
  purpose: CategoryPurpose;
  purposeLabel: string;
}

export interface Transaction {
  id: string;
  description: string;
  value: number;
  type: TransactionType;
  typeLabel: string;
  category: Category;
  person: Person;
}

export interface PersonTotals {
  personId: string;
  personName: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface Summary {
  byPerson: PersonTotals[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandBalance: number;
}

export interface CategoryTotals {
  categoryId: string;
  categoryDescription: string;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface CategorySummary {
  byCategory: CategoryTotals[];
  grandTotalIncome: number;
  grandTotalExpense: number;
  grandBalance: number;
}