using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webservice.Model
{
    public class Post
    {
        [JsonPropertyName("id")]
        [Required]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("authorId")]
        public int AuthorId { get; set; }
        [JsonPropertyName("price")]
        public int Price {get; set; }
    }
}
