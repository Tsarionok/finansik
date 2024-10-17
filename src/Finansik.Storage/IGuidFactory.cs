namespace Finansik.Storage;

public interface IGuidFactory
{ 
    Guid Create();
}

internal sealed class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}