namespace DemoFuncionario.Tests;

public class AddressBehavior {
    [Fact]
    public void Throws_with_empty_street() {
        Assert.Throws<ArgumentException>(() => new Address(string.Empty));
    }
    
    // not really required, but...
    [Fact]
    public void Initializes_fields() {
        var street = "Street";
        var zipCode = "1234-123";
        var civilParish = "Someplace";
        var address = new Address(street, zipCode, civilParish);
        
        Assert.Equal(street, address.Street);
        Assert.Equal(zipCode, address.ZipCode);
        Assert.Equal(civilParish, address.CivilParish);
    }
}
