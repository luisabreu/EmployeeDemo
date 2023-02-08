using System.Collections.Immutable;
using System.ComponentModel;
using System.Transactions;
using Dapper;
using DemoFuncionario.Employees;
using DemoFuncionario.Employees.Data;
using Microsoft.Data.SqlClient;

namespace DemoFuncionario.Tests.Employees.Data; 

public class EmployeeRepositoryBehavior {

    private const string _cnnString = "initial catalog=EmployeeDemo; data source=localhost; user id=employee; password=employee";

    [Fact]
    [Category("db")]
    public async Task Should_save_to_db() {
        var employee = new Employee("test", "123456789");
        var newAddress = new Address("Street", "123", "Funchal");
        employee.UpdateAddress(newAddress);
        employee.AddPhone("123123123");
        employee.AddEmail("teste@mail.pt");

        using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var cnn = new SqlConnection(_cnnString);
        await cnn.OpenAsync( );
        

        var rep = new EmployeeRepository(cnn);
        var saved = await rep.SaveAsync(employee);
        
        Assert.True(saved);

        var loadedEmployee = await cnn.QueryFirstAsync<EmployeeData>("select EmployeeId, Version, Name, TaxNumber, Street, ZipCode, CivilParish from employees where employeeid=@id",
            new { employee.Id});
        
        Assert.NotNull(loadedEmployee);
        Assert.Equal("test", loadedEmployee.Name);
        Assert.Equal("123456789", loadedEmployee.TaxNumber);
        Assert.Equal(newAddress.Street, loadedEmployee.Street);
        Assert.Equal(newAddress.ZipCode, loadedEmployee.ZipCode);
        Assert.Equal(newAddress.CivilParish, loadedEmployee.CivilParish);
        
        var contacts = await cnn.QueryAsync<ContactData>("select * from contacts where employeeid=@id",
                                                         new { employee.Id });
        
        Assert.NotNull(contacts);
        Assert.Contains(contacts.ToImmutableArray(  ), c => c is { Value: "123123123", ContactType    : ContactType.Phone });
        Assert.Contains(contacts.ToImmutableArray(  ), c => c is { Value: "teste@mail.pt", ContactType: ContactType.Email });

    }
    
    [Fact]
    [Category("db")]
    public async Task Should_load_from_db() {
        using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        await using var cnn = new SqlConnection(_cnnString);
        await cnn.OpenAsync( );
        
        
        var employee = new EmployeeData {
                                            Name = "test",
                                            TaxNumber = "123456789",
                                            CivilParish = "Some",
                                            Street = "Street",
                                            ZipCode = "1234-123"
                                        };
        var contacts = new[] {
                                 new ContactData { Value = "123123123", ContactType = ContactType.Phone },
                                 new ContactData { Value = "test@mail.pt", ContactType = ContactType.Email }
                             };

        var id = await cnn.QueryFirstAsync<int>("insert into employees (Name, TaxNumber, Street, ZipCode, CivilParish) values(@Name, @TaxNumber, @Street, @ZipCode, @CivilParish); select scope_identity()", 
                                                employee);

        await cnn.ExecuteAsync("insert into Contacts (Value, ContactType, EmployeeId) values (@Value, @ContactType, @EmployeeId)",
                               contacts.Select(c => new {
                                                            c.Value,
                                                            ContactType = (int)c.ContactType,
                                                            EmployeeId = id
                                                        }));
        
        

        var rep = new EmployeeRepository(cnn);
        var loadedEmployee = (await rep.GetAsync(id))!.GetInternalState();
        
        Assert.Equal("test", loadedEmployee.Name);
        Assert.Equal("123456789", loadedEmployee.TaxNumber);
        Assert.Equal("Street", loadedEmployee.Street);
        Assert.Equal("1234-123", loadedEmployee.ZipCode);
        Assert.Equal("Some", loadedEmployee.CivilParish);


        var loadedContacts = loadedEmployee.Contacts.ToImmutableArray( );
        Assert.Contains(loadedContacts, c => c is { Value: "123123123", ContactType    : ContactType.Phone });
        Assert.Contains(loadedContacts, c => c is { Value: "test@mail.pt", ContactType: ContactType.Email });

    }
}
