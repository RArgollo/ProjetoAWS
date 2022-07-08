namespace ProjetoAWS.lib.Models
{
    public class Usuario
    {
        public virtual int Id { get; private set; }
        public virtual string Email { get; private set; }
        public virtual string Cpf { get; private set; }
        public virtual DateTime DataNascimento { get; private set; }
        public virtual string Nome { get; private set; }
        public virtual string Senha { get; private set; }
        public virtual string UrlImagemCadastro { get; private set; }
        public virtual DateTime DataCriacao { get; private set; }

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

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetEmail(string email)
        {
            if (email.Contains("@"))
                Email = email;
        }

        public void SetCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.Contains(".") || cpf.Contains("-"))
            {

            }
            else
                Cpf = cpf;
        }

        public void SetDataNascimento(string dataNascimento)
        {
            var dataEmDateTime = DateTime.Parse(dataNascimento);
            if (dataEmDateTime.Year < 2010)
                DataNascimento = dataEmDateTime;
        }

        public void SetNome(string nome)
        {
            Nome = nome;
        }

        public void SetSenha(string senha)
        {
            if (senha.Length >= 8)
                Senha = senha;
        }

        public void SetDataCriacao(string dataCriacao)
        {
            DataCriacao = DateTime.Parse(dataCriacao);
        }

        public void SetUrlImagemCadastro(string urlImagemCadastro)
        {
            UrlImagemCadastro = urlImagemCadastro;
        }
    }
}