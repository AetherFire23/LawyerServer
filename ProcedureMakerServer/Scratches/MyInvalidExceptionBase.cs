namespace ProcedureMakerServer.Scratches;

public abstract class MyInvalidExceptionBase : Exception
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; } = null;

    public abstract int StatusCode { get; set; }

    public MyInvalidExceptionBase()
    {
        
    }

    public MyInvalidExceptionBase(object? obj)
    {
        this.Data = obj;
    }
}
