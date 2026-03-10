using CasaFinancas.Application.DTOs;
using CasaFinancas.Application.Services;
using CasaFinancas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CasaFinancas.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController(PersonService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try { return Ok(await service.GetByIdAsync(id)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto dto)
    {
        try
        {
            var result = await service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePersonDto dto)
    {
        try { return Ok(await service.UpdateAsync(id, dto)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await service.DeleteAsync(id); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }
}

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        try { return Created(string.Empty, await service.CreateAsync(dto)); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }
}

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(TransactionService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        try { return Created(string.Empty, await service.CreateAsync(dto)); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }
}

[ApiController]
[Route("api/[controller]")]
public class SummaryController(SummaryService service) : ControllerBase
{
    [HttpGet("by-person")]
    public async Task<IActionResult> ByPerson() =>
        Ok(await service.GetByPersonAsync());

    [HttpGet("by-category")]
    public async Task<IActionResult> ByCategory() =>
        Ok(await service.GetByCategoryAsync());
}
