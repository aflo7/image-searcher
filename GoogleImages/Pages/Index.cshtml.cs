using System.Collections;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoogleImages.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string Message { get; private set; } = "defaullt";
    private readonly HttpClient _httpClient;


    public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;

    }

    public async Task OnGetAsync() { 

        var searchQuery = Request.Query["searchQuery"];
    
        if (string.IsNullOrEmpty(searchQuery))
        {

            Message = "search query is empty!" +  searchQuery;
            return;
        }

        var url = $"https://serpapi.com/search.json?engine=google&q={searchQuery}&location=Austin%2C+Texas%2C+United+States&google_domain=google.com&gl=us&hl=en&tbm=isch&api_key=50620cfc21a9f435feaa7a3919e8d37033c867b9e2a17f9506be30524abbc0ee";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(content);

        if (data == null)
        {
            return;
        }

        if (data.length == 0)
        {
            return;
        }

        dynamic images_results = data.images_results;
        dynamic suggested_searches = data.suggested_searches;



        ViewData["images_results"] = images_results;
        ViewData["suggested_searches"] = suggested_searches;

        Console.WriteLine(images_results);
        return;
    }

}

