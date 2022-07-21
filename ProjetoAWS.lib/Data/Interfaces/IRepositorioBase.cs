namespace ProjetoAWS.lib.Data.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<List<T>> GetTodosAsync();
        Task AddAsync(T item);
        Task DeletarAsync(int id);
        Task<T> GetPorId(int id);
    }
}