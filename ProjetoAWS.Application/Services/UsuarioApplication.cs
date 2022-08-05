using Microsoft.AspNetCore.Http;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Models;
using ProjetoAWS.Services;

namespace ProjetoAWS.Application.Services
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IUsuarioServices _services;

        public UsuarioApplication(IUsuarioRepositorio repositorio, IUsuarioServices services)
        {
            _repositorio = repositorio;
            _services = services;
        }

        public async Task<Guid> CadastrarUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario(usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
            await _repositorio.AddAsync(usuario);
            return usuario.Id;
        }
        public async Task<Guid> Login(string email, string senha)
        {
            var usuarios = await _repositorio.GetTodosAsync();
            var usuarioAVerificar = usuarios.First(x => x.Email == email);
            if (usuarioAVerificar.VerificarSenhaHash(senha, usuarioAVerificar.Salt, usuarioAVerificar.Senha))
            {
                return usuarioAVerificar.Id;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var resposta = await _repositorio.GetTodosAsync();
            return resposta;
        }

        public async Task AtualizarSenha(Guid id, string senha)
        {
            await _repositorio.AtualizarSenha(id, senha);
        }

        public async Task DeletarUsuario(Guid id)
        {
            await _repositorio.DeletarAsync(id);
        }

        public async Task CadastrarImagem(Guid id, IFormFile imagem)
        {
            await _services.CadastrarImagem(id, imagem);
        }

        public async Task LoginImagem(Guid id, IFormFile foto)
        {
            await _services.LoginImagem(id, foto);
        }

        public async Task DeletarImagem(string nomeArquivo)
        {
            await _services.DeletarImagem(nomeArquivo);
        }
    }
}