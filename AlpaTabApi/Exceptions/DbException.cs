namespace AlpaTabApi.Exceptions;

public class DbExceptionWrapper : Exception
{
    public DbExceptionWrapper()
    {
    }

    public DbExceptionWrapper(string message)
        : base(message)
    {
    }

    public DbExceptionWrapper(string message, Exception inner)
        : base(message, inner)
    {
    }
}
