using Microsoft.AspNetCore.Mvc;
using ProjetoAWS.Application;
using ProjetoAWS.Application.Services;

namespace ProjetoAWS.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplication _application;
        public UsuarioController(IUsuarioApplication application)
        {
            _application = application;
        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario(UsuarioDTO usuarioDTO)
        {
            await _application.CadastrarUsuario(usuarioDTO);
            return Ok("Usuario cadastrado");
        }

        [HttpPost("CadastrarImagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            await _application.CadastrarImagem(id, imagem);
            return Ok("Imagem cadastrada");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
                await _application.Login(email, senha);
                return Ok("Login válido");
        }

        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile fotoLogin)
        {
                await _application.LoginImagem(id, fotoLogin);
                return Ok("Login válido");
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
                var resposta = await _application.GetUsuarios();
                return Ok(resposta);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarSenha(int id, string senha)
        {
                await _application.AtualizarSenha(id, senha);
                return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
                await _application.DeletarUsuario(id);
                return Ok();
        }

        [HttpDelete("DeletarImagem")]
        public async Task<IActionResult> DeletarImagem(string nomeArquivo)
        {

                await _application.DeletarImagem(nomeArquivo);
                return Ok();
        }
    }
}