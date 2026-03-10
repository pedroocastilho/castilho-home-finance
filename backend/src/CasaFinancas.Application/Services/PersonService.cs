using CasaFinancas.Application.DTOs;
using CasaFinancas.Domain.Entities;
using CasaFinancas.Domain.Interfaces;

namespace CasaFinancas.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciamento de pessoas.
/// Coordena a lógica entre os repositórios, delegando regras de negócio às entidades de domínio.
/// </summary>
public class PersonService(IPersonRepository repository)
{
    public async Task<IEnumerable<PersonDto>> GetAllAsync()
    {
        var people = await repository.GetAllAsync();
        return people.Select(ToDto);
    }

    public async Task<PersonDto> GetByIdAsync(Guid id)
    {
        var person = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        return ToDto(person);
    }

    public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
    {
        var person = new Person(dto.Name, dto.Age);
        await repository.AddAsync(person);
        return ToDto(person);
    }

    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        person.Update(dto.Name, dto.Age);
        await repository.UpdateAsync(person);
        return ToDto(person);
    }

    public async Task DeleteAsync(Guid id)
    {
        var person = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        // O cascade delete das transações é garantido pela configuração do EF Core
        await repository.DeleteAsync(person);
    }

    private static PersonDto ToDto(Person p) =>
        new(p.Id, p.Name, p.Age, p.IsMinor);
}
