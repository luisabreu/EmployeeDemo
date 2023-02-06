public class EmployeeData {
    public int EmployeeId { get; set; }

    public string Name { get; set; } = "";

    public string TaxNumber { get; set; } = "";

    public string? Street { get; set; }
    
    public string? ZipCode { get; set; }
    
    public string? CivilParish { get; set; }

    public byte[] Version { get; set; } = Array.Empty<byte>( );

    public IList<ContactData> Contacts { get; set; } = new List<ContactData>( );
}

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
