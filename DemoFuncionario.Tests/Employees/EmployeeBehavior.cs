using DemoFuncionario.Core;
using DemoFuncionario.Employees;

namespace DemoFuncionario.Tests.Employees; 

public partial class EmployeeBehavior {
   
   private const string _employeeName = "Test";
   private const string _taxNumber = "123456789";
   private static readonly Address _address = new Address("Somewhere", "1100", "Parish");
   private static Employee GenerateEmployee(string name, string taxNumber, Address? address) => new Employee(name, taxNumber, address);
   

   [Fact]
   public void Instances_equals_if_both_dont_have_same_id_but_equals_name_taxnumber() {
      var empl1 = new Employee("test", "123456789");
      var empl2 = new Employee("test", "123456789");
      
      Assert.Equal(empl1, empl2);
   }
   
   [Fact]
   public void Instances_different_if_both_dont_have_same_id_but_different_name_same_taxnumber() {
      var empl1 = new Employee("test", "123456789");
      var empl2 = new Employee("test2", "123456789");
      
      Assert.NotEqual(empl1, empl2);
   }

   [Fact]
   public void Create_new_employee() {
      var employee = GenerateEmployee(_employeeName, _taxNumber, _address);

      var evt = employee.Events.Last( ) as EmployeeCreated;
      Assert.NotNull(evt);
      Assert.Equal(_employeeName, evt.Name);
      Assert.Equal(_taxNumber, evt.TaxNumber);
      Assert.Equal(_address, evt.Address);
   }

   [Fact]
   public void Updating_address_generates_Event() {
      var employee = GenerateEmployee(_employeeName, _taxNumber, _address);

      var newAddress = new Address("Otherplace", "1100-101", "Parish");
      employee.UpdateAddress(newAddress);

      var evt = employee.Events.Last( ) as AddressUpdated;
      Assert.NotNull(evt);
      Assert.Equal(newAddress, evt.Address);
   }

   [Fact]
   public void Adding_contact_generates_event() {
      var employee = GenerateEmployee(_employeeName, _taxNumber, _address);

      var ct = Contact.CreateEmail("test@mail.pt");
      employee.AddContact(ct);

      var evt = employee.Events.Last( ) as ContactAdded;
      Assert.NotNull(evt);
      Assert.Equal(ct, evt.contact);
   }
   
   [Fact]
   public void Removing_contact_generates_event() {
      var employee = GenerateEmployee(_employeeName, _taxNumber, _address);
      var ct = Contact.CreateEmail("test@mail.pt");
      employee.AddContact(ct);

      employee.RemoveContact(ct);
      
      var evt = employee.Events.Last( ) as ContactRemoved;
      Assert.NotNull(evt);
      Assert.Equal(ct, evt.contact);
   }
}
