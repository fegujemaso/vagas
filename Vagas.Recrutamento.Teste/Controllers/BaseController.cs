using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Vagas.Recrutamento.Teste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected virtual T ProcessarJsonParametro<T>(JToken jToken) where T : new()
        {
            if (jToken == null)
                return default(T);

            return this.ProcessarJsonParametro<T>(jToken.ToString(), new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }

        protected virtual T ProcessarJsonParametro<T>(string jsonParametro) where T : new()
        {
            if (string.IsNullOrEmpty(jsonParametro))
                return default(T);

            return ProcessarJsonParametro<T>(jsonParametro, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }

        protected virtual T ProcessarJsonParametro<T>(JToken jToken, IsoDateTimeConverter dateTimeConverter) where T : new()
        {
            if (jToken == null)
                return default(T);

            return ProcessarJsonParametro<T>(jToken.ToString(), dateTimeConverter);
        }

        protected virtual T ProcessarJsonParametro<T>(string jsonParametro, IsoDateTimeConverter dateTimeConverter) where T : new()
        {
            if (string.IsNullOrEmpty(jsonParametro))
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonParametro, dateTimeConverter);
        }
    }
}