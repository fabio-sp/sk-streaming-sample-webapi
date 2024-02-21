using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

namespace sample_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("azure-open-ai")]
    public async IAsyncEnumerable<string> GetWithAzureOpenAI()
    {
        var prompt = "Write a short poem about cats";

        var kernel = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion("deployment-name", "endpoint", "apiKey").Build();

        var textCompletionService = kernel.GetRequiredService<ITextGenerationService>();
        
        await foreach (var textStreamingResult in textCompletionService.GetStreamingTextContentsAsync(prompt))
        {
            yield return textStreamingResult.Text;
        }
    }

    [HttpGet("open-ai")]
    public async IAsyncEnumerable<string> GetWithOpenAI()
    {
        var prompt = "Write a short poem about cats";

        var kernel = Kernel.CreateBuilder().AddOpenAIChatCompletion("model-id", "api-key").Build();

        var textCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        
        await foreach (var textStreamingResult in textCompletionService.GetStreamingChatMessageContentsAsync(prompt))
        {
            yield return textStreamingResult.Content;
        }
    }
}
