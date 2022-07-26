using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using ProjetoAWS.lib.Data.Interfaces;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace ProjetoAWS.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly IAmazonS3 _amazonS3;
        private static readonly List<string> _extensoesImagem = new List<string>() { "image/jpeg", "image/png" };
        public UsuarioServices(IUsuarioRepositorio repositorio, AmazonRekognitionClient client, IAmazonS3 amazonS3)
        {
            _rekognitionClient = client;
            _amazonS3 = amazonS3;
            _repositorio = repositorio;
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

        public async Task LoginImagem(int id, IFormFile foto)
        {
            var usuario = await _repositorio.GetPorId(id);
            var comparacao = await CompararRosto(usuario.UrlImagemCadastro, foto);
            if (comparacao.FaceMatches.Count != 1)
                throw new Exception();
        }

        public async Task DeletarImagem(string nomeArquivo)
        {
            await _amazonS3.DeleteObjectAsync("imagens-teste-dodev", nomeArquivo);
        }

        public async Task<DetectFacesResponse> AnalisarRosto(string nomeArquivo)
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

        public async Task<CompareFacesResponse> CompararRosto(string nomeArquivoS3, IFormFile fotoLogin)
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

        public async Task<PutObjectResponse> AddImagem(IFormFile imagem)
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
    }
}