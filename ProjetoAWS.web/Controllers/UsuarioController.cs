using Microsoft.AspNetCore.Mvc;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Models;
using ProjetoAWS.web.DTos;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;

namespace ProjetoAWS.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly IAmazonS3 _amazonS3;
        private static readonly List<string> _extensoesImagem = new List<string>() { "image/jpeg", "image/png" };
        public UsuarioController(IUsuarioRepositorio repositorio, AmazonRekognitionClient client, IAmazonS3 amazonS3)
        {
            _repositorio = repositorio;
            _rekognitionClient = client;
            _amazonS3 = amazonS3;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var resposta = await _repositorio.GetTodosAsync();
            return Ok(resposta);
        }

        [HttpPost("Cadastro")]
        public async Task<IActionResult> Cadastro(int id, string email, string cpf, string dataNascimento, string nome, string senha, string dataCriacao)
        {
            var usuarioDTO = new UsuarioDTO(){
                Id = id,
                Email = email,
                Cpf = cpf,
                DataNascimento = dataNascimento,
                Nome = nome,
                Senha = senha,
                DataCriacao = dataCriacao
            }; 
            var usuario = new Usuario(usuarioDTO.Id, usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
            await _repositorio.AddAsync(usuario);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarSenha(int id, string senha)
        {
            await _repositorio.AtualizarSenha(id, senha);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            await _repositorio.DeletarAsync(id);
            return Ok();
        }

        [HttpPost("CadastrarImagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            var fotoUsuario = await AddImagem(imagem);
            await AnalisarRosto(imagem.FileName);
            await _repositorio.AtualizarUrlFotoCadastro(id, imagem.FileName);
            return Ok("Imagem cadastrada");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuarios = await _repositorio.GetTodosAsync();
            var usuarioAVerificar = usuarios.First(x => x.Email == email);
            if (usuarioAVerificar.Senha == senha)
            {
                return Ok(usuarioAVerificar.Id);
            }
            else
            {
                return BadRequest("Email ou senha inválidos");
            }
        }

        private async Task<IActionResult> AddImagem(IFormFile imagem)
        {
            if (!_extensoesImagem.Contains(imagem.ContentType))
                return BadRequest("Tipo Invalido");
            using (var streamDaImagem = new MemoryStream())
            {
                await imagem.CopyToAsync(streamDaImagem);

                var request = new PutObjectRequest();
                request.Key = imagem.FileName;
                request.BucketName = "imagens-teste-dodev";
                request.InputStream = streamDaImagem;

                var resposta = await _amazonS3.PutObjectAsync(request);
                return Ok(resposta);
            }
        }

        private async Task<IActionResult> AnalisarRosto(string nomeArquivo)
        {
            var entrada = new DetectFacesRequest();
            var imagem = new Image();

            var s3Object = new Amazon.Rekognition.Model.S3Object()
            {
                Bucket = "imagens-teste-dodev",
                Name = nomeArquivo
            };

            imagem.S3Object = s3Object;
            entrada.Image = imagem;
            entrada.Attributes = new List<string>() { "ALL" };

            var resposta = await _rekognitionClient.DetectFacesAsync(entrada);

            if (resposta.FaceDetails.Count == 1 && resposta.FaceDetails.First().Eyeglasses.Value == false)
            {
                return Ok(resposta);
            }
            else
            {
                await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", nomeArquivo);
                return BadRequest("Imagem inválida");
            }

        }

        [HttpDelete("DeletarImagem")]
        public async Task<IActionResult> DeletarImagem(string nomeArquivo)
        {
            await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", nomeArquivo);
            return Ok("Imagem deletada");
        }
    }
}