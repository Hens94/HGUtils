namespace HGUtils.Exceptions.Models
{
    public class ExceptionInfo
    {
        public int Code { get; set; } = 999;
        public string UserMessage { get; set; } = "Ha ocurrido un error no controlado";
        public ExceptionInfoDetail ExceptionInfoDetail { get; set; }
    }
}
