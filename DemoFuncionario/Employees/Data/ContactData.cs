namespace DemoFuncionario.Employees.Data;

public class ContactData {
    public string Value { get; set; } = "";
    
    public ContactType ContactType { get; set; }

    protected bool Equals(ContactData other) {
        return Value == other.Value && ContactType == other.ContactType;
    }

    public override bool Equals(object? obj) {
        if( ReferenceEquals(null, obj) ) {
            return false;
        }

        if( ReferenceEquals(this, obj) ) {
            return true;
        }

        if( obj.GetType( ) != this.GetType( ) ) {
            return false;
        }

        return Equals((ContactData)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Value, (int)ContactType);
    }

    public static bool operator ==(ContactData? left, ContactData? right) {
        return Equals(left, right);
    }

    public static bool operator !=(ContactData? left, ContactData? right) {
        return !Equals(left, right);
    }
}