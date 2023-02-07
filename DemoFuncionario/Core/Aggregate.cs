using System.Collections.Immutable;

namespace DemoFuncionario.Core;

public abstract class Aggregate : Entity {
    private readonly IList<DomainEvent> _events = new List<DomainEvent>( );

    public IEnumerable<DomainEvent> Events => _events.ToImmutableArray( );
    
    public abstract byte[]? Version { get; }

    protected override bool Equals(Entity other) {
        var otherAggregate = other as Aggregate;
        if( otherAggregate is null ) {
            return false;
        }

        var emptyByteArray = Array.Empty<byte>( );
        return ( Version?.SequenceEqual(otherAggregate.Version ?? emptyByteArray) ?? false) && base.Equals(other);
    }
}
