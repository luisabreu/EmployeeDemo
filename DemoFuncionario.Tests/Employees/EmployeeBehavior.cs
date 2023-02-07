using DemoFuncionario.Employees;

namespace DemoFuncionario.Tests.Employees; 

public partial class EmployeeBehavior {

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
}
