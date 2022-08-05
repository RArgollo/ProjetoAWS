using Microsoft.AspNetCore.Http;
namespace ProjetoAWS.Services
{
    public interface IUsuarioServices
    {
         Task CadastrarImagem(Guid id, IFormFile imagem);
         Task LoginImagem(Guid id, IFormFile foto);
         Task DeletarImagem(string nomeArquivo);
        
    }
}