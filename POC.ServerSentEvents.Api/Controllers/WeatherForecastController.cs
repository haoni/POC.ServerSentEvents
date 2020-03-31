using System.Text;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace POC.ServerSentEvents.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            for(int i=0; i < Summaries.Length; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                var mensagem = $"data: {i+1} {Summaries[i]}\r\n";
                byte[] mensagemBytes = ASCIIEncoding.ASCII.GetBytes(mensagem);
                await Response.Body.WriteAsync(mensagemBytes, 0, mensagemBytes.Length);
                await Response.Body.FlushAsync();
            }
        }
    }
}