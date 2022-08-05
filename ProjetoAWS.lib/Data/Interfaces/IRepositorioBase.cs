namespace ProjetoAWS.lib.Data.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<List<T>> GetTodosAsync();
        Task AddAsync(T item);
        Task DeletarAsync(Guid id);
        Task<T> GetPorId(Guid id);
    }
}