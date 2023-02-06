namespace DemoFuncionario.Tests; 

public class EmployeeBehavior {

    [Theory]
    [InlineData("", "123456789")]
    [InlineData("Someone", "")]
    public void Throws_with_invalid_values(string name, string nif) {
        Assert.Throws<ArgumentException>(() => new Employee(name, nif));
    }

    [Fact]
    public void Adds_contact_if_not_defined() {
        var employee = new Employee("Test", "123456789");
        employee.AddPhone("123123123");
        
        Assert.Equal(Contact.CreatePhone("123123123"), employee.Contacts.First());
    }
    
}
