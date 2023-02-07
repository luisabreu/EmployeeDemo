using DemoFuncionario.Employees.Data;

namespace DemoFuncionario.Employees; 

public record Contact{
    public Contact(string value, ContactType contactType) {
        CheckIfContactIsValid(value, contactType);
        Value = value;
        ContactType = contactType;
    }

    // implicit conversion to simplify getting contact from contactdata
    public  static implicit operator Contact(ContactData data) => new Contact(data.Value, data.ContactType);

    public static implicit operator ContactData(Contact contact) => new ContactData {
                                                                                        Value = contact.Value,
                                                                                        ContactType = contact.ContactType
                                                                                    };
    
    private void CheckIfContactIsValid(string value, ContactType contactType) {
        // do nothing, might throw if contact's
        // value is not valid for type
    }

    public string Value { get; init; }
    
    public ContactType ContactType { get; init; }

    public static Contact CreatePhone(string value) => new Contact(value, ContactType.Phone);
    
    public static Contact CreateEmail(string value) => new Contact(value, ContactType.Email);
}
