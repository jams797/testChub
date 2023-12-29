namespace Consultorio_Seguros.Models.Model
{
    public class ResponseJson
    {
        public string Message { get; set; }
        public bool error { get; set; }
        public IEnumerable<object> Data { get; set; }
    }
}
