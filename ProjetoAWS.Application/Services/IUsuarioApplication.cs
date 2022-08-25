using Microsoft.AspNetCore.Http;
using ProjetoAWS.lib.Models;

namespace ProjetoAWS.Application.Services
{
    public interface IUsuarioApplication
    {
        Task<Guid> CadastrarUsuario(UsuarioDTO dto);
        Task<Guid> Login(string email, string senha);
        Task<List<Usuario>> GetUsuarios();
        Task AtualizarSenha(Guid id, string senha);
        Task DeletarUsuario(Guid id);
        Task CadastrarImagem(Guid id, IFormFile imagem);
        Task LoginImagem(Guid id, IFormFile foto);
        Task DeletarImagem(string nomeArquivo);
        
    }
}