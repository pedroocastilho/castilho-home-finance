using CasaFinancas.Domain.Entities;
using CasaFinancas.Domain.Enums;
using Xunit;

namespace CasaFinancas.Tests;

/// <summary>
/// Testes unitários das regras de negócio do domínio.
/// Testamos diretamente as entidades — sem dependência de banco ou infraestrutura.
/// </summary>
public class PersonTests
{
    [Fact]
    public void Person_ShouldBeMinor_WhenAgeIsUnder18()
    {
        var person = new Person("João", 15);
        Assert.True(person.IsMinor);
    }

    [Fact]
    public void Person_ShouldNotBeMinor_WhenAgeIs18()
    {
        var person = new Person("Maria", 18);
        Assert.False(person.IsMinor);
    }

    [Fact]
    public void Person_ShouldThrow_WhenNameExceeds200Chars()
    {
        var longName = new string('A', 201);
        Assert.Throws<DomainException>(() => new Person(longName, 25));
    }

    [Fact]
    public void Person_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<DomainException>(() => new Person("", 25));
    }
}

public class CategoryTests
{
    [Theory]
    [InlineData(CategoryPurpose.Expense, TransactionType.Expense, true)]
    [InlineData(CategoryPurpose.Expense, TransactionType.Income, false)]
    [InlineData(CategoryPurpose.Income, TransactionType.Income, true)]
    [InlineData(CategoryPurpose.Income, TransactionType.Expense, false)]
    [InlineData(CategoryPurpose.Both, TransactionType.Expense, true)]
    [InlineData(CategoryPurpose.Both, TransactionType.Income, true)]
    public void Category_Compatibility_ShouldMatchPurpose(
        CategoryPurpose purpose, TransactionType type, bool expected)
    {
        var category = new Category("Teste", purpose);
        Assert.Equal(expected, category.IsCompatibleWith(type));
    }
}

public class TransactionTests
{
    private readonly Person _adultPerson = new("Carlos", 30);
    private readonly Person _minorPerson = new("Ana", 16);
    private readonly Category _expenseCategory = new("Alimentação", CategoryPurpose.Expense);
    private readonly Category _incomeCategory = new("Salário", CategoryPurpose.Income);
    private readonly Category _bothCategory = new("Transferência", CategoryPurpose.Both);

    [Fact]
    public void Transaction_ShouldThrow_WhenMinorTriesToRegisterIncome()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new Transaction("Freelance", 500, TransactionType.Income, _incomeCategory, _minorPerson));

        Assert.Contains("Menores de idade", ex.Message);
    }

    [Fact]
    public void Transaction_ShouldAllow_WhenMinorRegistersExpense()
    {
        var transaction = new Transaction("Lanche", 20, TransactionType.Expense, _expenseCategory, _minorPerson);
        Assert.Equal(20, transaction.Value);
    }

    [Fact]
    public void Transaction_ShouldThrow_WhenCategoryIsIncompatible()
    {
        Assert.Throws<DomainException>(() =>
            new Transaction("Salário", 3000, TransactionType.Income, _expenseCategory, _adultPerson));
    }

    [Fact]
    public void Transaction_ShouldThrow_WhenValueIsZeroOrNegative()
    {
        Assert.Throws<DomainException>(() =>
            new Transaction("Teste", 0, TransactionType.Expense, _expenseCategory, _adultPerson));
    }

    [Fact]
    public void Transaction_ShouldAllow_BothCategoryWithAnyType()
    {
        var expense = new Transaction("PIX enviado", 100, TransactionType.Expense, _bothCategory, _adultPerson);
        var income = new Transaction("PIX recebido", 200, TransactionType.Income, _bothCategory, _adultPerson);

        Assert.Equal(100, expense.Value);
        Assert.Equal(200, income.Value);
    }
}
