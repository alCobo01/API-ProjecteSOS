using System.Text.Json.Serialization;

namespace API_SOS_Code.DTOs.Json
{
    public class JsonPackage
    {
        public int Amount { get; set; }
        public JsonPackageContent Content { get; set; } = new JsonPackageContent();
    }
}
