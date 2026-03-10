using CasaFinancas.Application.DTOs;
using CasaFinancas.Domain.Entities;
using CasaFinancas.Domain.Enums;
using CasaFinancas.Domain.Interfaces;

namespace CasaFinancas.Application.Services;

/// <summary>
/// Serviço de aplicação para transações financeiras.
/// Orquestra a busca de dependências (pessoa e categoria) antes de criar a transação,
/// deixando as validações de negócio para as entidades de domínio.
/// </summary>
public class TransactionService(
    ITransactionRepository transactionRepository,
    IPersonRepository personRepository,
    ICategoryRepository categoryRepository)
{
    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        var transactions = await transactionRepository.GetAllAsync();
        return transactions.Select(ToDto);
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
    {
        var person = await personRepository.GetByIdAsync(dto.PersonId)
            ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        var category = await categoryRepository.GetByIdAsync(dto.CategoryId)
            ?? throw new KeyNotFoundException("Categoria não encontrada.");

        // A entidade Transaction aplica todas as regras de negócio no construtor
        var transaction = new Transaction(dto.Description, dto.Value, dto.Type, category, person);
        await transactionRepository.AddAsync(transaction);

        return ToDto(transaction);
    }

    private static TransactionDto ToDto(Transaction t) => new(
        t.Id,
        t.Description,
        t.Value,
        t.Type,
        t.Type == TransactionType.Expense ? "Despesa" : "Receita",
        new CategoryDto(t.Category.Id, t.Category.Description, t.Category.Purpose, t.Category.Purpose.ToString()),
        new PersonDto(t.Person.Id, t.Person.Name, t.Person.Age, t.Person.IsMinor)
    );
}
