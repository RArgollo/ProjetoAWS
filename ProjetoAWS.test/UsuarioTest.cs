using Xunit;
using ProjetoAWS.lib.Models;
using System;

namespace ProjetoAWS.test
{
    public class UsuarioTest
    {

        private Usuario CriaUsuarioPadrao()
        {
            return new Usuario("rafael@email", "12345678978", "19/11/2001", "rafael", "rafa1902", "07/07/2022");
        }
        
        [Fact]
        public void TesteSetEmail()
        {
            var usuario = CriaUsuarioPadrao();
            var emailEsperado = "rafael@email";
            Assert.Equal(emailEsperado, usuario.Email);
        }
        [Fact]
        public void TesteSetCpf()
        {
            var usuario = CriaUsuarioPadrao();
            var cpfEsperado = "12345678978";
            Assert.Equal(cpfEsperado, usuario.Cpf);
        }
        [Fact]
        public void TesteSetDataNascimento()
        {
            var usuario = CriaUsuarioPadrao();
            var dataNascimentoEsperada = DateTime.Parse("19/11/2001");
            Assert.Equal(dataNascimentoEsperada, usuario.DataNascimento);
        }
        [Fact]
        public void TesteSetNome()
        {
            var usuario = CriaUsuarioPadrao();
            var nomeEsperado = "rafael";
            Assert.Equal(nomeEsperado, usuario.Nome);
        }
        /*[Fact]
        public void TesteSetSenha()
        {
            var usuario = CriaUsuarioPadrao();
            var senhaEsperada = "rafa1902";
            Assert.Equal(senhaEsperada, usuario.Senha);
        }
        */
        [Fact]
        public void TesteSetDataCriacao()
        {
            var usuario = CriaUsuarioPadrao();
            var dataCriacaoEsperada = DateTime.Parse("07/07/2022");
            Assert.Equal(dataCriacaoEsperada, usuario.DataCriacao);
        }
    }
}