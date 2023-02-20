using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DemoFuncionario.Core;

namespace DemoFuncionario.Employees.Services; 

public interface ISerializer {
    Task<string> Serialize(DomainEvent evt);
}

public class Serializer: ISerializer {
    private readonly JsonSerializerOptions _options;

    public Serializer(JsonSerializerOptions? options = null) {
        _options = options ??
                   new JsonSerializerOptions {
                                                 PropertyNameCaseInsensitive = true,
                                                 TypeInfoResolver = new DomainEventTypeInfoResolver(  )
                                             };
    }

    public Task<string> Serialize(DomainEvent evt) {
        var serialized = JsonSerializer.Serialize(evt, _options);
        return Task.FromResult(serialized );
    }
}

public class DomainEventTypeInfoResolver : DefaultJsonTypeInfoResolver {
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options) {
        var jsonTypeInfo = base.GetTypeInfo(type, options);
        if( jsonTypeInfo.Type == typeof( DomainEvent ) ) {
            jsonTypeInfo.PolymorphismOptions = new( ) {
                                                          IgnoreUnrecognizedTypeDiscriminators = true,
                                                          UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                                          DerivedTypes = {
                                                                             new JsonDerivedType(typeof( EmployeeCreated ), "ec"),
                                                                             new JsonDerivedType(typeof( AddressUpdated ), "au"),
                                                                             new JsonDerivedType(typeof( ContactAdded ), "ca"),
                                                                             new JsonDerivedType(typeof( ContactRemoved ), "cr")
                                                                         }
                                                      };
        }
        else if( jsonTypeInfo.Type == typeof( EmployeeCreated ) ) {
            jsonTypeInfo.PolymorphismOptions = new( ) {
                                                          IgnoreUnrecognizedTypeDiscriminators = true,
                                                          UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                                          DerivedTypes = {
                                                                             new JsonDerivedType(typeof( EmployeeCreated ), "ec")
                                                                         }
                                                      };
        }
        else if( jsonTypeInfo.Type == typeof( AddressUpdated ) ) {
            jsonTypeInfo.PolymorphismOptions = new( ) {
                                                          IgnoreUnrecognizedTypeDiscriminators = true,
                                                          UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                                          DerivedTypes = {
                                                                             new JsonDerivedType(typeof( AddressUpdated ), "au")
                                                                         }
                                                      };
        }
        else if( jsonTypeInfo.Type == typeof( ContactAdded ) ) {
            jsonTypeInfo.PolymorphismOptions = new( ) {
                                                          IgnoreUnrecognizedTypeDiscriminators = true,
                                                          UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                                          DerivedTypes = {
                                                                             new JsonDerivedType(typeof( ContactAdded ), "ca")
                                                                         }
                                                      };
        }
        else if( jsonTypeInfo.Type == typeof( ContactRemoved ) ) {
            jsonTypeInfo.PolymorphismOptions = new( ) {
                                                          IgnoreUnrecognizedTypeDiscriminators = true,
                                                          UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                                          DerivedTypes = {
                                                                             new JsonDerivedType(typeof( ContactRemoved ), "cr")
                                                                         }
                                                      };
        }
        return jsonTypeInfo;
    }
}
