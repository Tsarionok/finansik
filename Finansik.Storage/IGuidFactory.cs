namespace Finansik.Storage;

public interface IGuidFactory
{ 
    Guid Create();
}

internal class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}