namespace DemoFuncionario.Employees; 

public record Address(string Street, string? ZipCode = null, string? CivilParish = null) {

    // can always use custom primary ctor and perform additional logic if required
    public string Street { get; init; } = string.IsNullOrEmpty(Street)
                                              ? throw new ArgumentException(nameof(Street))
                                              : Street;
    
}
