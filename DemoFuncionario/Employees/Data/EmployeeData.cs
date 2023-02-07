namespace DemoFuncionario.Employees.Data;

public class EmployeeData {
    public int EmployeeId { get; set; }

    public string Name { get; set; } = "";

    public string TaxNumber { get; set; } = "";

    public string? Street { get; set; }
    
    public string? ZipCode { get; set; }
    
    public string? CivilParish { get; set; }

    public byte[] Version { get; set; } = Array.Empty<byte>( );

    public IList<ContactData> Contacts { get; set; } = new List<ContactData>( );
}
