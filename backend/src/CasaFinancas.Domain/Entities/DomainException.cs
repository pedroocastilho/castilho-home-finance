namespace CasaFinancas.Domain.Entities;

/// <summary>
/// Exceção de domínio — usada para sinalizar violações das regras de negócio.
/// Separar exceções de domínio das de infraestrutura facilita o tratamento na camada de API.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
