using Microsoft.EntityFrameworkCore;
using ProjetoAWS.lib.Models;
using ProjetoAWS.lib.Data.Interfaces;

namespace ProjetoAWS.lib.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        private readonly AWSContext _context;

        public UsuarioRepositorio(AWSContext context) : base(context, context.Usuarios)
        {
            _context = context;
        }
        public async Task AtualizarSenha(int id, string senha)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            usuario.SetSenha(senha);
            _context.SaveChangesAsync();
        }
    }
}