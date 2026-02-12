using PostStore.Models;

namespace PostStore.Requests;

public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = "en-us";
    public List<Translation> Translations { get; set; } = [];
}