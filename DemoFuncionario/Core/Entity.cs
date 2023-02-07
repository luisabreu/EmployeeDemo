namespace DemoFuncionario.Core; 

public abstract class Entity {
    public abstract int Id { get; }

    protected virtual bool Equals(Entity other) {
        return Id == other.Id ;
    }

    public override bool Equals(object? obj) {
        if( ReferenceEquals(null, obj) ) {
            return false;
        }

        if( ReferenceEquals(this, obj) ) {
            return true;
        }

        if( obj.GetType( ) != GetType( ) ) {
            return false;
        }

        return Equals((Entity)obj);
    }

    public override int GetHashCode() {
        return Id;
    }

    public static bool operator ==(Entity? left, Entity? right) {
        return Equals(left, right);
    }

    public static bool operator !=(Entity? left, Entity? right) {
        return !Equals(left, right);
    }
}
