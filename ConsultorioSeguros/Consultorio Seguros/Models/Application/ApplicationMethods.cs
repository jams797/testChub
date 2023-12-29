using Consultorio_Seguros.Models.Model;
using Newtonsoft.Json;

namespace Consultorio_Seguros.Models.Application
{
    public class ApplicationMethods
    {

        //Obtiene las datos de la carpeta Configuration
        public static dynamic GetFieldJson(string field, string code)
        {
            var myJsonString = File.ReadAllText("Configuration/json/" + field + ".json");
            var myJObject = JsonConvert.DeserializeObject<dynamic>(myJsonString);
            if (code != null) myJObject = myJObject[code];
            return myJObject;
        }

        //Devuelve un objeto 
        public ResponseJson ReturnJsonError(string code, bool isCode = true)
        {
            ResponseJson resp = new ResponseJson();
            resp.error = true;
            resp.Data = null;
            resp.Message = (isCode) ? getMessage(code) : code;
            return resp;
        }

        //Obtiene el mensaje de las configuraciones segun el parametro recibido
        public static string getMessage(string code)
        {
            try
            {
                var myJsonString = File.ReadAllText("Configuration/json/messages.json");
                var myJObject = JsonConvert.DeserializeObject<dynamic>(myJsonString);
                return myJObject[code];
            }
            catch (Exception ex)
            {
                return "----------";
            }
        }
    }
}
