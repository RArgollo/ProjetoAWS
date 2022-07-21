namespace ProjetoAWS.Application.Services
{
    public interface IUsuarioApplication
    {
        Task CadastrarUsuario(UsuarioDTO dto);
    }
}