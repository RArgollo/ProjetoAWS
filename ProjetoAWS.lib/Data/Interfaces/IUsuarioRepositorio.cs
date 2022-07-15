using ProjetoAWS.lib.Models;

namespace ProjetoAWS.lib.Data.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
         Task AtualizarSenha(int id, string senha);
         Task AtualizarUrlFotoCadastro(int id, string urlImagemCadastro);
    }
}