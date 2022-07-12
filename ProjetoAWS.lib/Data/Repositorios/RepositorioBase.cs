using Microsoft.EntityFrameworkCore;
using ProjetoAWS.lib.Data.Interfaces;

namespace ProjetoAWS.lib.Data.Repositorios
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        private readonly AWSContext _context;
        private readonly DbSet<T> _dbset;

        public RepositorioBase(AWSContext context, DbSet<T> dbset)
        {
            _context = context;
            _dbset = dbset;
        }
        public async Task<List<T>> GetTodosAsync()
        {
            var resposta = await _dbset.AsNoTracking().ToListAsync();
            return resposta;
        }

        public async Task AddAsync(T item)
        {
            await _dbset.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var item = await _dbset.FindAsync(id);
            _dbset.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}