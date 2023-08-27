using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StaffSalary.API.Middleware.Model
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
