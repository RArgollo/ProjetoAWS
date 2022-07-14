using Microsoft.AspNetCore.Mvc;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace ProjetoAWS.web.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RekognitionController : ControllerBase
    {
        private readonly AmazonRekognitionClient _rekognitionClient;

        public RekognitionController(AmazonRekognitionClient rekognitionClient)
        {
            _rekognitionClient = rekognitionClient;
        }
        [HttpGet]
        public async Task<IActionResult> AnalisarRosto(string nomeArquivo)
        {
            var entrada = new DetectFacesRequest();
            var imagem = new Image();

            var s3Object = new S3Object()
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
                return BadRequest();
            }

        }

    }
}