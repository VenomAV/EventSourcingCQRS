namespace EventSourcingCQRS.Domain.Core
{
    public interface IAggregateId
    {
        string IdAsString();
    }
}
