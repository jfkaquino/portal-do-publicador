using Microsoft.EntityFrameworkCore;

namespace PortalDoPublicador.Shared.Infrastructure.Data;

public interface IRepository
{
    Task<List<T>> ObterTodosAsync<T>() where T : class;
    IQueryable<T> Query<T>() where T : class;
    Task<T?> ObterPorIdAsync<T>(Guid id) where T : class;
    Task<T?> InserirAsync<T>(T entidade) where T : class;
    Task<T?> AtualizarAsync<T>(T entidade) where T : class;
    Task ExcluirAsync<T>(Guid id) where T : class;
}

public class Repository(SharedDbContext context) : IRepository
{
    public async Task<List<T>> ObterTodosAsync<T>() where T : class
    {
        return await context.Set<T>().ToListAsync();
    }

    public IQueryable<T> Query<T>() where T : class
    {
        return context.Set<T>();
    }

    public async Task<T?> ObterPorIdAsync<T>(Guid id) where T : class
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> InserirAsync<T>(T entidade) where T : class
    {
        context.Set<T>().Add(entidade);
        await context.SaveChangesAsync();
        return entidade;
    }

    public async Task<T?> AtualizarAsync<T>(T entidade) where T : class
    {
        context.Set<T>().Update(entidade);
        await context.SaveChangesAsync();
        return entidade;
    }

    public async Task ExcluirAsync<T>(Guid id) where T : class
    {
        var entidade = await ObterPorIdAsync<T>(id);
        if (entidade != null)
        {
            context.Set<T>().Remove(entidade);
            await context.SaveChangesAsync();
        }
    }
}