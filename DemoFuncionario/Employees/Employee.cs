using System.Collections.Immutable;
using DemoFuncionario.Core;
using DemoFuncionario.Employees.Data;

namespace DemoFuncionario.Employees;

public class Employee: Aggregate {
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
        
        AddEvent(new EmployeeCreated(name, taxNumber, address));


    }
    
    public void UpdateAddress(Address newAddress) {
        _data.Street = newAddress.Street;
        _data.ZipCode = newAddress.ZipCode;
        _data.CivilParish = newAddress.CivilParish;
        
        AddEvent(new AddressUpdated(newAddress));
    }

    // used for testing since there are no events
    public IEnumerable<Contact> Contacts => _data.Contacts.Select(c => new Contact(c.Value, c.ContactType)).ToImmutableArray( );
    
    public override int Id => _data.EmployeeId;

    public override byte[]? Version => _data.Version; 

    protected override bool Equals(Entity other) {
        if( other is not Employee otherEmployee ) {
            return false;
        }
        
        if( other.Id == 0 && Id == 0 ) {
            return string.Compare(otherEmployee._data.Name, _data.Name)            == 0 &&
                   string.Compare(otherEmployee._data.TaxNumber, _data.TaxNumber ) == 0;

        }
        return base.Equals(other);
    }

    public void AddContact(string value, ContactType contactType) => AddContact(new Contact(value, contactType));

    public void AddContact(Contact ct) {
        if( _data.Contacts.Contains(ct) ) {
            return;
        }
        _data.Contacts.Add(ct);
        
        AddEvent(new ContactAdded(ct));
    }

    internal EmployeeData GetInternalState() => _data;

    public void RemoveContact(string value, ContactType contactType) => RemoveContact(new Contact(value, contactType));

    public void RemoveContact(Contact ct) {
        if( _data.Contacts.Remove(ct) ) {
            AddEvent(new ContactRemoved(ct));
        }
    }

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
