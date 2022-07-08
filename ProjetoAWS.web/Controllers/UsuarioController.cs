using Microsoft.AspNetCore.Mvc;
using ProjetoAWS.lib;

namespace ProjetoAWS.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private static List<Usuario> Usuarios {get; set; } = new List<Usuario>();

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(Usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(Usuario usuario)
        {
            Usuarios.Add(usuario);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarSenha(int id, string senha)
        {
            var usuario = Usuarios.Find(x => x.Id == id);
            usuario.SetSenha(senha);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var usuario = Usuarios.Find(x => x.Id == id);
            Usuarios.Remove(usuario);
            return Ok();
        }
    }
}