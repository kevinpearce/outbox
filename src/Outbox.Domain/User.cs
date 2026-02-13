using Outbox.Domain.Interfaces;

namespace Outbox.Domain;

public sealed class User : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public string Name { get; private init; }
    public Guid Id { get; private init; }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    private User(string name, Guid id)
    {
        Name = name;
        Id = id;
    }
    
    public static User Create(string name)
    {
        var user = new User(name, Guid.NewGuid());
        user._domainEvents.Add(new UserCreatedEvent(user.Id, name));
        return user;
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}