public record Contact{
    public Contact(string value, ContactType contactType) {
        CheckIfContactIsValid(value, contactType);
        Value = value;
        ContactType = contactType;
    }

    private void CheckIfContactIsValid(string value, ContactType contactType) {
        // do nothing, might throw if contact's
        // value is not valid for type
    }

    public string Value { get; init; }
    
    public ContactType ContactType { get; init; }

    public static Contact CreatePhone(string value) => new Contact(value, ContactType.Phone);
    
    public static Contact CreateEmail(string value) => new Contact(value, ContactType.Email);
}
