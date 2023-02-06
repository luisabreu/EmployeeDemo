using System.Collections.Immutable;

public class Employee {
    private string _name;
    private string _taxNumber;
    private Address? _address;
    private List<Contact> _contacts = new List<Contact>( );

    public Employee(string name, string taxNumber, Address? address = null) {
        CheckIfNameIsValid(name);
        CheckIfTaxNumberIsValid(taxNumber);

        _name = name;
        _taxNumber = taxNumber;
        _address = address;
    }
    
    public void UpdateAddress(Address newAddress) => _address = newAddress;
    
    // used for testing since there are no events
    public IEnumerable<Contact> Contacts => _contacts.ToImmutableArray( );

    public void AddContact(string value, ContactType contactType) => AddContact(new Contact(value, contactType));

    public void AddContact(Contact ct) {
        if( _contacts.Contains(ct) ) {
            return;
        }
        _contacts.Add(ct);
    }

    public void RemoveContact(string value, ContactType contactType) => RemoveContact(new Contact(value, contactType));

    public void RemoveContact(Contact ct) => _contacts.Remove(ct);
    
    private void CheckIfTaxNumberIsValid(string taxNumber) {
        if( string.IsNullOrEmpty(taxNumber) ) {
            throw new ArgumentException(taxNumber);
        }
        // can run additional logic for checking if everythin is ok
    }

    private void CheckIfNameIsValid(string name) {
        if( string.IsNullOrEmpty(name) ) {
            throw new ArgumentException(name);
        }
    }
    
}

public static class EmployeeExtensions {
    public static void AddPhone(this Employee employee, string value) => employee.AddContact(value, ContactType.Phone);

    public static void AddEmail(this Employee employee, string value) => employee.AddContact(value, ContactType.Email);
}
