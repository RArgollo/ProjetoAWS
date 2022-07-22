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
            try
            {
                await _application.CadastrarUsuario(usuarioDTO);
                return Ok("Usuario cadastrado");
            }
            catch
            {
                var ex = new Exception("Usuario inválido");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CadastrarImagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            try
            {
                await _application.CadastrarImagem(id, imagem);
                return Ok("Imagem cadastrada");
            }
            catch
            {
                var ex = new Exception("Imagem inválida");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
            try
            {
                await _application.Login(email, senha);
                return Ok("Login válido");
            }
            catch
            {
                var ex = new Exception("Email ou senha inválidos");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile fotoLogin)
        {
            try
            {
                await _application.LoginImagem(id, fotoLogin);
                return Ok("Login válido");
            }
            catch
            {
                var ex = new Exception("Foto de login ou Id inválidos");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var resposta = await _application.GetUsuarios();
                return Ok(resposta);
            }
            catch
            {
                var ex = new Exception("Não foi possível carregar a lsita de usuários");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarSenha(int id, string senha)
        {
            try
            {
                await _application.AtualizarSenha(id, senha);
                return Ok();
            }
            catch
            {
                var ex = new Exception("Id ou senha inválidos");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                await _application.DeletarUsuario(id);
                return Ok();
            }
            catch
            {
                var ex = new Exception("Id inválido");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletarImagem")]
        public async Task<IActionResult> DeletarImagem(string nomeArquivo)
        {
            try
            {
                await _application.DeletarImagem(nomeArquivo);
                return Ok();
            }
            catch
            {
                var ex = new Exception("Nome de arquivo inválido");
                return BadRequest(ex.Message);
            }
        }
    }
}