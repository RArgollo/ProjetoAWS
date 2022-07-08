using Microsoft.AspNetCore.Mvc;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Data.Repositorios;
using ProjetoAWS.lib.Models;
using ProjetoAWS.web.DTos;

namespace ProjetoAWS.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repositorio;

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var resposta = _repositorio.GetTodosAsync();
            return Ok(resposta);
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario(usuarioDTO.Id, usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
            await _repositorio.AddAsync(usuario);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarSenha(int id, string senha)
        {
            var resposta = _repositorio.AtualizarSenha(id, senha);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var resposta = _repositorio.DeletarAsync(id);
            return Ok();
        }
    }
}