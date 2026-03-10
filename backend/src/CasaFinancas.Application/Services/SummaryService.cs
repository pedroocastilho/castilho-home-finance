using CasaFinancas.Application.DTOs;
using CasaFinancas.Domain.Enums;
using CasaFinancas.Domain.Interfaces;

namespace CasaFinancas.Application.Services;

/// <summary>
/// Serviço responsável pelos totalizadores do sistema.
/// Calcula receitas, despesas e saldo por pessoa e por categoria.
/// </summary>
public class SummaryService(
    IPersonRepository personRepository,
    ICategoryRepository categoryRepository,
    ITransactionRepository transactionRepository)
{
    public async Task<SummaryDto> GetByPersonAsync()
    {
        var people = await personRepository.GetAllAsync();
        var transactions = await transactionRepository.GetAllAsync();

        var byPerson = people.Select(person =>
        {
            var personTransactions = transactions.Where(t => t.PersonId == person.Id);

            var income = personTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value);

            var expense = personTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value);

            return new PersonTotalsDto(person.Id, person.Name, income, expense, income - expense);
        }).ToList();

        return new SummaryDto(
            byPerson,
            byPerson.Sum(p => p.TotalIncome),
            byPerson.Sum(p => p.TotalExpense),
            byPerson.Sum(p => p.Balance)
        );
    }

    public async Task<CategorySummaryDto> GetByCategoryAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        var transactions = await transactionRepository.GetAllAsync();

        var byCategory = categories.Select(category =>
        {
            var categoryTransactions = transactions.Where(t => t.CategoryId == category.Id);

            var income = categoryTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value);

            var expense = categoryTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value);

            return new CategoryTotalsDto(category.Id, category.Description, income, expense, income - expense);
        }).ToList();

        return new CategorySummaryDto(
            byCategory,
            byCategory.Sum(c => c.TotalIncome),
            byCategory.Sum(c => c.TotalExpense),
            byCategory.Sum(c => c.Balance)
        );
    }
}
