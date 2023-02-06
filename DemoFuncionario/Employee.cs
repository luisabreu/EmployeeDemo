using System.Collections.Immutable;
using System.Collections.ObjectModel;

public class Employee {
    private readonly EmployeeData _data;

    
    internal Employee(EmployeeData employeeData) {
        // loading from db, skip checks
        _data = employeeData;
    }
    
    public Employee(string name, string taxNumber, Address? address = null) {
        CheckIfNameIsValid(name);
        CheckIfTaxNumberIsValid(taxNumber);

        _data = new EmployeeData {
                                     Name = name,
                                     TaxNumber = taxNumber,
                                     Street = address?.Street ?? "",
                                     CivilParish = address?.CivilParish,
                                     ZipCode = address?.ZipCode
                                 };


    }
    
    public void UpdateAddress(Address newAddress) {
        _data.Street = newAddress.Street;
        _data.ZipCode = newAddress.ZipCode;
        _data.CivilParish = newAddress.CivilParish;
    }

    // used for testing since there are no events
    public IEnumerable<Contact> Contacts => _data.Contacts.Select(c => new Contact(c.Value, c.ContactType)).ToImmutableArray( );
    
    public int EmployeeId => _data.EmployeeId;

    public void AddContact(string value, ContactType contactType) => AddContact(new Contact(value, contactType));

    public void AddContact(Contact ct) {
        if( _data.Contacts.Contains(ct) ) {
            return;
        }
        _data.Contacts.Add(ct);
    }

    internal EmployeeData GetInternalState() => _data;

    public void RemoveContact(string value, ContactType contactType) => RemoveContact(new Contact(value, contactType));

    public void RemoveContact(Contact ct) => _data.Contacts.Remove(ct);
    
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
