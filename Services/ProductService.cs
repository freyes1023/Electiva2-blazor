using System.Net.Http.Json;
using System.Text.Json;
using System.Text;


namespace blazor;

public class ProductService : IProductService
{
    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;
    private List<Product> products;
    
    // public ProductService(HttpClient httpClient, JsonSerializerOptions optionsJson)
    public ProductService(HttpClient httpClient)
    {
        client = httpClient;
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };        
    }
    public async Task<List<Product>?> Get()
    {
        var response = await client.GetAsync("v1/products");
        var content = await response.Content.ReadAsStringAsync();
        if(!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
        
        return JsonSerializer.Deserialize<List<Product>>(content, options);
    }

    public async Task Add(Product product)
    {
        // Configurar opciones de serialización para que las claves estén en minúsculas
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true // Esto es opcional para una mejor legibilidad del JSON
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(product, options), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("v1/products", jsonContent);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }            
    }

    public async Task Update(Product product)
    {
        // Configurar opciones de serialización para que las claves estén en minúsculas
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true // Esto es opcional para una mejor legibilidad del JSON
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(product, options), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("v1/products/"+ product.Id, jsonContent);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }            
    }

    public async Task Delete(int productId)
    {
        var response = await client.DeleteAsync($"v1/products/{productId}");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
    }

    public async Task<Product> GetById(int productId)
    {
        var response = await client.GetAsync($"v1/products/{productId}");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
         {
             throw new ApplicationException(content);
         }
        return JsonSerializer.Deserialize<Product>(content, options);
    }
}

public interface IProductService
{
    Task<List<Product>?> Get();
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(int productId);
    Task<Product> GetById(int productId);
}