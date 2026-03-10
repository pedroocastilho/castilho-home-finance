using CasaFinancas.Domain.Enums;

namespace CasaFinancas.Domain.Entities;

/// <summary>
/// Categoria de uma transação.
/// A finalidade define quais tipos de transação podem utilizar esta categoria.
/// </summary>
public class Category
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public CategoryPurpose Purpose { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Category() { }

    public Category(string description, CategoryPurpose purpose)
    {
        Id = Guid.NewGuid();
        SetDescription(description);
        Purpose = purpose;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Descrição da categoria é obrigatória.");

        if (description.Length > 400)
            throw new DomainException("Descrição não pode ter mais de 400 caracteres.");

        Description = description.Trim();
    }

    /// <summary>
    /// Verifica se a categoria é compatível com o tipo de transação informado.
    /// Uma categoria "Both" aceita qualquer tipo.
    /// </summary>
    public bool IsCompatibleWith(TransactionType type)
    {
        return Purpose == CategoryPurpose.Both
            || (Purpose == CategoryPurpose.Expense && type == TransactionType.Expense)
            || (Purpose == CategoryPurpose.Income && type == TransactionType.Income);
    }
}
