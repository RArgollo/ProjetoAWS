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
        public async Task<IActionResult> Cadastro(UsuarioDTO usuarioDTO)
        {
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
            await AddImagem(imagem);
            var resposta = await AnalisarRosto(imagem.FileName);
            if (resposta.FaceDetails.Count != 1 || resposta.FaceDetails.First().Eyeglasses.Value == true)
            {
                await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", imagem.FileName);
                return BadRequest("Imagem inválida");
            }
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

        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile fotoLogin)
        {
            var usuario = await _repositorio.GetPorId(id);
            var comparacao = await CompararRosto(usuario.UrlImagemCadastro, fotoLogin);
            if (comparacao.FaceMatches.Count == 1)
            {
                return Ok("Foto de login Válida");
            }
            else
            {
                return BadRequest("Foto de login inválida");
            }
        }

        [HttpDelete("DeletarImagem")]
        public async Task<IActionResult> DeletarImagem(string nomeArquivo)
        {
            await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", nomeArquivo);
            return Ok("Imagem deletada");
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

        private async Task<DetectFacesResponse> AnalisarRosto(string nomeArquivo)
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
            return resposta;
        }

        private async Task<CompareFacesResponse> CompararRosto(string nomeArquivoS3, IFormFile fotoLogin)
        {
            using (var memoryStream = new MemoryStream())
            {
                var request = new CompareFacesRequest();

                var requestSource = new Image()
                {
                    S3Object = new Amazon.Rekognition.Model.S3Object()
                    {
                        Bucket = "imagens-teste-dodev",
                        Name = nomeArquivoS3
                    }
                };

                await fotoLogin.CopyToAsync(memoryStream);
                var requestTarget = new Image()
                {
                    Bytes = memoryStream
                };

                request.SourceImage = requestSource;
                request.TargetImage = requestTarget;

                var response = await _rekognitionClient.CompareFacesAsync(request);
                return response;
            }

        }
    }
}