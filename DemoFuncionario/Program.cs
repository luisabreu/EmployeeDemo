// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Transactions;
using DemoFuncionario.Employees;
using DemoFuncionario.Employees.Data;
using Microsoft.Data.SqlClient;


const string cnnString = "data source=localhost; initial catalog=employeedemo; user id=employee; password=employee";


var employeeId = await CreateEmployeeAsync( );

if( employeeId < 0 ) {
    Console.WriteLine("Error inserting employee");
    return;
}

var employee = await LoadEmployee(employeeId);
if( employee is null ) {
    Console.WriteLine("Couldn't load employee from db");
    return;
}
PrintEmployeeData(employee);


if( await UpdateEmployee(employeeId) ) {
    var anotherEmployee = await LoadEmployee(employeeId);
    PrintEmployeeData(employee);
}

void PrintEmployeeData(Employee employee) {
    Console.WriteLine("Employee data");
    var state = employee.GetInternalState( );
    Console.WriteLine($"{state.Name} - {state.TaxNumber} - {state.Street} - {state.ZipCode} - {state.CivilParish} -{Encoding.UTF8.GetString(state.Version)} ");
    foreach( var ct in state.Contacts ) {
        Console.WriteLine($"{ct.Value} - {ct.ContactType}");
    }
}

async Task<bool> UpdateEmployee(int employeeId) {
    var employee = await LoadEmployee(employeeId);
    if( employee is null ) {
        throw new InvalidOperationException( );
    }
    employee.RemoveContact("123123123", ContactType.Phone);

    return await SaveOrUpdateEmployeeAsync(employee);
}

async Task<Employee?> LoadEmployee(int employeeId) {
    using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    await using var cnn = new SqlConnection(cnnString);
    await cnn.OpenAsync( );

    var rep = new EmployeeRepository(cnn);
    var employee = await rep.GetAsync(employeeId);
    return employee;
}

async Task<int> CreateEmployeeAsync() {
    var employee = new Employee("name", "123", new Address("somewhere"));

    employee.UpdateAddress(new Address("Blah", "9100", "Civil parish"));
    employee.AddPhone("123123123");
    employee.AddEmail("teste@mail.pt");

    if( await SaveOrUpdateEmployeeAsync(employee) ) {
        return employee.Id;
    }

    return -1;
}

async Task<bool> SaveOrUpdateEmployeeAsync(Employee employee) {
    using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    await using var cnn = new SqlConnection(cnnString);
    await cnn.OpenAsync( );

    var rep = new EmployeeRepository(cnn);
    if( await rep.SaveAsync(employee) ) {
        tran.Complete(  );
        return true;
    }

    return false;
}
