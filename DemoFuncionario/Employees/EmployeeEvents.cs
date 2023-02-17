using DemoFuncionario.Employees;

namespace DemoFuncionario.Core; 

public record EmployeeCreated(string Name, 
                              string TaxNumber,
                              Address? Address = null): DomainEvent;

public record ContactAdded(Contact contact): DomainEvent;

public record ContactRemoved(Contact contact): DomainEvent;

public record AddressUpdated(Address? Address): DomainEvent;
