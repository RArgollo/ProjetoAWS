using Microsoft.AspNetCore.Http;
using ProjetoAWS.lib.Models;

namespace ProjetoAWS.Application.Services
{
    public interface IUsuarioApplication
    {
        Task CadastrarUsuario(UsuarioDTO dto);
        Task CadastrarImagem(int id, IFormFile imagem);
        Task<int> Login(string email, string senha);
        Task LoginImagem(int id, IFormFile foto);
        Task<List<Usuario>> GetUsuarios();
        Task AtualizarSenha(int id, string senha);
        Task DeletarUsuario(int id);
        Task DeletarImagem(string nomeArquivo);
    }
}