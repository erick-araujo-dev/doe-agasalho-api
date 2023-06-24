using System.Text.Json.Serialization;

namespace DoeAgasalhoApiV2._0.Models
{
    public class UpdateUsernameModel

    {
        [JsonIgnore]
        public int? Id { get; set; }
        
        public string? Nome { get; set; }

    }
}
