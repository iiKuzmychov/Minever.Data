using System.Text.Json.Serialization;

namespace Minever.Data.Generators;

public partial class BlocksGenerator
{
    internal class Block
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
    }
}