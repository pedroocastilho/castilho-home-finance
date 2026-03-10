using CasaFinancas.Domain.Entities;
using CasaFinancas.Domain.Interfaces;
using CasaFinancas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CasaFinancas.Infrastructure.Persistence.Repositories;

public class PersonRepository(AppDbContext db) : IPersonRepository
{
    public async Task<IEnumerable<Person>> GetAllAsync() =>
        await db.People.AsNoTracking().ToListAsync();

    public async Task<Person?> GetByIdAsync(Guid id) =>
        await db.People.FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Person person)
    {
        db.People.Add(person);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        db.People.Update(person);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        db.People.Remove(person);
        await db.SaveChangesAsync();
    }
}

public class CategoryRepository(AppDbContext db) : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await db.Categories.AsNoTracking().ToListAsync();

    public async Task<Category?> GetByIdAsync(Guid id) =>
        await db.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task AddAsync(Category category)
    {
        db.Categories.Add(category);
        await db.SaveChangesAsync();
    }
}

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
    public async Task<IEnumerable<Transaction>> GetAllAsync() =>
        await db.Transactions
            .Include(t => t.Person)
            .Include(t => t.Category)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Transaction?> GetByIdAsync(Guid id) =>
        await db.Transactions
            .Include(t => t.Person)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Transaction transaction)
    {
        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();
    }
}
