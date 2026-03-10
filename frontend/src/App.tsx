import { Link, Route, BrowserRouter as Router, Routes, useLocation } from "react-router-dom";
import CategoriesPage from "./pages/CategoriesPage";
import PeoplePage from "./pages/PeoplePage";
import SummaryPage from "./pages/SummaryPage";
import TransactionsPage from "./pages/TransactionsPage";

const NAV = [
  { to: "/", label: "Resumo" },
  { to: "/pessoas", label: "Pessoas" },
  { to: "/categorias", label: "Categorias" },
  { to: "/transacoes", label: "Transações" },
];

function Navbar() {
  const { pathname } = useLocation();
  return (
    <nav className="bg-white shadow-sm border-b">
      <div className="max-w-4xl mx-auto px-6 flex gap-6 h-14 items-center">
        <span className="font-bold text-blue-600 mr-4">Castilho Home Finance</span>
        {NAV.map((n) => (
          <Link
            key={n.to}
            to={n.to}
            className={`text-sm font-medium pb-1 border-b-2 transition-colors ${
              pathname === n.to
                ? "border-blue-600 text-blue-600"
                : "border-transparent text-gray-500 hover:text-gray-800"
            }`}
          >
            {n.label}
          </Link>
        ))}
      </div>
    </nav>
  );
}

export default function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <Routes>
          <Route path="/" element={<SummaryPage />} />
          <Route path="/pessoas" element={<PeoplePage />} />
          <Route path="/categorias" element={<CategoriesPage />} />
          <Route path="/transacoes" element={<TransactionsPage />} />
        </Routes>
      </div>
    </Router>
  );
}
