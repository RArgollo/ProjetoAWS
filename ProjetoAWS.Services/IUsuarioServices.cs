using Microsoft.AspNetCore.Http;
namespace ProjetoAWS.Services
{
    public interface IUsuarioServices
    {
         Task CadastrarImagem(int id, IFormFile imagem);
         Task LoginImagem(int id, IFormFile foto);
         Task DeletarImagem(string nomeArquivo);
        
    }
}