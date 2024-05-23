using System.Text.Json.Serialization;
using System.Text.Json;

namespace BackOfficeSilicon.Models;

public class DynamicGraphQLResponse
{
    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
}
