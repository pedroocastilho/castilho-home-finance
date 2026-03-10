using CasaFinancas.Application.DTOs;
using CasaFinancas.Domain.Entities;
using CasaFinancas.Domain.Interfaces;

namespace CasaFinancas.Application.Services;

public class CategoryService(ICategoryRepository repository)
{
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await repository.GetAllAsync();
        return categories.Select(ToDto);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category(dto.Description, dto.Purpose);
        await repository.AddAsync(category);
        return ToDto(category);
    }

    private static CategoryDto ToDto(Category c) =>
        new(c.Id, c.Description, c.Purpose, c.Purpose.ToString());
}
