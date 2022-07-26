using Microsoft.AspNetCore.Http;
using ProjetoAWS.lib.Models;

namespace ProjetoAWS.Application.Services
{
    public interface IUsuarioApplication
    {
        Task CadastrarUsuario(UsuarioDTO dto);
        Task<int> Login(string email, string senha);
        Task<List<Usuario>> GetUsuarios();
        Task AtualizarSenha(int id, string senha);
        Task DeletarUsuario(int id);
        Task CadastrarImagem(int id, IFormFile imagem);
        Task LoginImagem(int id, IFormFile foto);
        Task DeletarImagem(string nomeArquivo);
        
    }
}