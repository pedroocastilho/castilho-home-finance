namespace CasaFinancas.Domain.Entities;

/// <summary>
/// Entidade que representa uma pessoa no sistema.
/// Segue o padrão de entidade do DDD com encapsulamento das regras de negócio.
/// </summary>
public class Person
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Age { get; private set; }

    // Navegação para transações — cascade delete é configurado na infraestrutura
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    // Construtor sem parâmetros requerido pelo EF Core
    private Person() { }

    public Person(string name, int age)
    {
        Id = Guid.NewGuid();
        Update(name, age);
    }

    /// <summary>
    /// Atualiza os dados da pessoa, mantendo as validações centralizadas na entidade.
    /// </summary>
    public void Update(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome é obrigatório.");

        if (name.Length > 200)
            throw new DomainException("Nome não pode ter mais de 200 caracteres.");

        if (age < 0 || age > 150)
            throw new DomainException("Idade inválida.");

        Name = name.Trim();
        Age = age;
    }

    /// <summary>
    /// Menores de 18 anos só podem ter transações do tipo despesa.
    /// </summary>
    public bool IsMinor => Age < 18;
}
