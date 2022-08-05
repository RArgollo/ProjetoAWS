using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace ProjetoAWS.lib.Models
{
    public class Usuario : ModelBase
    {
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Nome { get; private set; }
        public byte[] Senha { get; private set; }
        public byte[] Salt { get; private set; }
        public string? UrlImagemCadastro { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Usuario(string email, string cpf, string dataNascimento, string nome, string senha, string dataCriacao) : base(Guid.NewGuid())
        {
            SetEmail(email);
            SetCpf(cpf);
            SetDataNascimento(dataNascimento);
            SetNome(nome);
            SetSenha(senha);
            SetDataCriacao(dataCriacao);
        }

        private Usuario() : base(Guid.NewGuid())
        {
            
        }

        public void SetEmail(string email)
        {
            if (email.Contains("@"))
            {
                Email = email;
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.Contains(".") || cpf.Contains("-"))
            {
                throw new Exception();
            }
            else
                Cpf = cpf;
        }

        public void SetDataNascimento(string dataNascimento)
        {
            var dataEmDateTime = DateTime.Parse(dataNascimento);
            if (dataEmDateTime.Year < 2010)
            {

                DataNascimento = DateTime.SpecifyKind(dataEmDateTime, DateTimeKind.Utc);
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetNome(string nome)
        {
            Nome = nome;
        }

        public void SetSenha(string senha)
        {
            if (senha.Length >= 8)
            {
                Salt = CreateSalt();
                var senhaHash = SenhaHash(senha, Salt);
                Senha = senhaHash;
            }
            else
            {
                throw new Exception();
            }
        }

        public void SetDataCriacao(string dataCriacao)
        {
            DataCriacao = DateTime.SpecifyKind(DateTime.Parse(dataCriacao), DateTimeKind.Utc);
        }

        public void SetUrlImagemCadastro(string urlImagemCadastro)
        {
            UrlImagemCadastro = urlImagemCadastro;
        }

        private byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        private byte[] SenhaHash(string senha, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(senha));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024*1024;

            return argon2.GetBytes(16);
        }

        public bool VerificarSenhaHash(string senha, byte[] salt, byte[] hash)
        {
            var novaSenhaHash = SenhaHash(senha, salt);
            return hash.SequenceEqual(novaSenhaHash);
        }

    }
}