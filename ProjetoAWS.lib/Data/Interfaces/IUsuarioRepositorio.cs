using ProjetoAWS.lib.Models;

namespace ProjetoAWS.lib.Data.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
         Task AtualizarSenha(Guid id, string senha);
         Task AtualizarUrlFotoCadastro(Guid id, string urlImagemCadastro);
    }
}