using DemoFuncionario.Employees;
using DemoFuncionario.Employees.Data;

namespace DemoFuncionario.Tests.Employees.Data; 

public class ContactDataBehavior {
    [Fact]
    public void Equality_works() {
        var ct1 = new ContactData {
                                      ContactType = ContactType.Email,
                                      Value = "teste@mail.pt"
                                  };
        var ct2 = new ContactData {
                                      ContactType = ContactType.Email,
                                      Value = "teste@mail.pt"
                                  };
        
        Assert.Equal(ct2, ct1);
    }
}
