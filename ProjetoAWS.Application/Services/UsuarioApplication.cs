using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Models;

namespace ProjetoAWS.Application.Services
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly IAmazonS3 _amazonS3;
        private static readonly List<string> _extensoesImagem = new List<string>() { "image/jpeg", "image/png" };

        //MÉTODOS PÚBLICOS

        public UsuarioApplication(IUsuarioRepositorio repositorio, AmazonRekognitionClient client, IAmazonS3 amazonS3)
        {
            _repositorio = repositorio;
            _rekognitionClient = client;
            _amazonS3 = amazonS3;
        }

        public async Task CadastrarUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario(usuarioDTO.Id, usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
            await _repositorio.AddAsync(usuario);
        }

        public async Task CadastrarImagem(int id, IFormFile imagem)
        {
            await AddImagem(imagem);
            var resposta = await AnalisarRosto(imagem.FileName);
            if (resposta.FaceDetails.Count != 1 || resposta.FaceDetails.First().Eyeglasses.Value == true)
            {
                await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", imagem.FileName);
                throw new Exception();
            }
            await _repositorio.AtualizarUrlFotoCadastro(id, imagem.FileName);
        }

        public async Task<int> Login(string email, string senha)
        {
            var usuarios = await _repositorio.GetTodosAsync();
            var usuarioAVerificar = usuarios.First(x => x.Email == email);
            if (usuarioAVerificar.Senha == senha)
            {
                return usuarioAVerificar.Id;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task LoginImagem(int id, IFormFile foto)
        {
            var usuario = await _repositorio.GetPorId(id);
            var comparacao = await CompararRosto(usuario.UrlImagemCadastro, foto);
            if (comparacao.FaceMatches.Count != 1) 
                throw new Exception();
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var resposta = await _repositorio.GetTodosAsync();
            return resposta;
        }

        public async Task AtualizarSenha(int id, string senha)
        {
            await _repositorio.AtualizarSenha(id, senha);
        }

        public async Task DeletarUsuario(int id)
        {
            await _repositorio.DeletarAsync(id);
        }

        public async Task DeletarImagem(string nomeArquivo)
        {
            await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", nomeArquivo);
        }

        //MÉTODOS PRIVADOS

        private async Task<PutObjectResponse> AddImagem(IFormFile imagem)
        {
            if (!_extensoesImagem.Contains(imagem.ContentType))
                throw new Exception("Tipo Invalido");
            using (var streamDaImagem = new MemoryStream())
            {
                await imagem.CopyToAsync(streamDaImagem);

                var request = new PutObjectRequest();
                request.Key = imagem.FileName;
                request.BucketName = "imagens-teste-dodev";
                request.InputStream = streamDaImagem;

                var resposta = await _amazonS3.PutObjectAsync(request);
                return resposta;
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