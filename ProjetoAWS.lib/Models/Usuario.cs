namespace ProjetoAWS.lib.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string? UrlImagemCadastro { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Usuario(int id, string email, string cpf, string dataNascimento, string nome, string senha, string dataCriacao)
        {
            SetId(id);
            SetEmail(email);
            SetCpf(cpf);
            SetDataNascimento(dataNascimento);
            SetNome(nome);
            SetSenha(senha);
            SetDataCriacao(dataCriacao);
        }
        public Usuario()
        {

        }

        public void SetId(int id)
        {
            Id = id;
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
                Senha = senha;
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
    }
}