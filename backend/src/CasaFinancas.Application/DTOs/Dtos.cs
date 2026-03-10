using CasaFinancas.Domain.Enums;

namespace CasaFinancas.Application.DTOs;

// ─── Person ───────────────────────────────────────────────────────────────────

public record CreatePersonDto(string Name, int Age);
public record UpdatePersonDto(string Name, int Age);

public record PersonDto(Guid Id, string Name, int Age, bool IsMinor);

// ─── Category ─────────────────────────────────────────────────────────────────

public record CreateCategoryDto(string Description, CategoryPurpose Purpose);

public record CategoryDto(Guid Id, string Description, CategoryPurpose Purpose, string PurposeLabel);

// ─── Transaction ──────────────────────────────────────────────────────────────

public record CreateTransactionDto(
    string Description,
    decimal Value,
    TransactionType Type,
    Guid CategoryId,
    Guid PersonId
);

public record TransactionDto(
    Guid Id,
    string Description,
    decimal Value,
    TransactionType Type,
    string TypeLabel,
    CategoryDto Category,
    PersonDto Person
);

// ─── Totals ───────────────────────────────────────────────────────────────────

public record PersonTotalsDto(
    Guid PersonId,
    string PersonName,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

public record SummaryDto(
    IEnumerable<PersonTotalsDto> ByPerson,
    decimal GrandTotalIncome,
    decimal GrandTotalExpense,
    decimal GrandBalance
);

public record CategoryTotalsDto(
    Guid CategoryId,
    string CategoryDescription,
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance
);

public record CategorySummaryDto(
    IEnumerable<CategoryTotalsDto> ByCategory,
    decimal GrandTotalIncome,
    decimal GrandTotalExpense,
    decimal GrandBalance
);
