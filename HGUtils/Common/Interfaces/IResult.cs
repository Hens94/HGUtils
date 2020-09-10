namespace HGUtils.Common.Interfaces
{
    public interface IResult
    {
        int Code { get; set; }
        string Message { get; set; }
        string DetailMessage { get; set; }
    }

    public interface IResult<T> : IResult where T : class
    {
        T Results { get; set; }
    }
}
