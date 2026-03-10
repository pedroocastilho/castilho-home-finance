using CasaFinancas.Domain.Enums;

namespace CasaFinancas.Domain.Entities;

/// <summary>
/// Representa uma transação financeira vinculada a uma pessoa e uma categoria.
/// As regras de negócio são aplicadas diretamente aqui, seguindo DDD.
/// </summary>
public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public TransactionType Type { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public Guid PersonId { get; private set; }
    public Person Person { get; private set; } = null!;

    private Transaction() { }

    public Transaction(string description, decimal value, TransactionType type, Category category, Person person)
    {
        Id = Guid.NewGuid();
        ApplyBusinessRules(description, value, type, category, person);
    }

    private void ApplyBusinessRules(string description, decimal value, TransactionType type, Category category, Person person)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Descrição da transação é obrigatória.");

        if (description.Length > 400)
            throw new DomainException("Descrição não pode ter mais de 400 caracteres.");

        if (value <= 0)
            throw new DomainException("O valor da transação deve ser positivo.");

        // Menores de 18 anos só podem registrar despesas
        if (person.IsMinor && type == TransactionType.Income)
            throw new DomainException("Menores de idade só podem registrar despesas.");

        // A categoria deve ser compatível com o tipo da transação
        if (!category.IsCompatibleWith(type))
            throw new DomainException($"A categoria '{category.Description}' não é compatível com o tipo '{type}'.");

        Description = description.Trim();
        Value = value;
        Type = type;
        Category = category;
        CategoryId = category.Id;
        Person = person;
        PersonId = person.Id;
    }
}
