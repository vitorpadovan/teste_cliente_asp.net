using Microsoft.AspNetCore.Mvc;

namespace CadastroClienteBff.Config.Exceptions
{
    public class ResponseData
    {
        public int codError { get; set; } = -1;
        public string Message { get; set; } = "";
        public bool Successful { get; set; }
        public List<ErrorModel> Error { get; set; } = new List<ErrorModel>();
        public int ErrorCount { get { return Error.Count; } private set { } }
        
        
    }
}
