using Dapper;
using DemoFuncionario.Core;
using DemoFuncionario.Employees.Services;
using Microsoft.Data.SqlClient;

namespace DemoFuncionario.Employees.Data; 

public class EmployeeRepository : IRepository<Employee> {
    private readonly SqlConnection _cnn;
    private readonly ISerializer _serializer;

    public EmployeeRepository(SqlConnection cnn, ISerializer serializer) {
        _cnn = cnn;
        _serializer = serializer;
    }

    public async Task<bool> SaveAsync(Employee employee) {
        // save to db
        var saved = employee.Id > 0
                   ? await UpdateEmployeeAsync(employee)
                   : await InsertEmployeeAsync(employee);
        
        employee.ClearEvents(  );

        return saved;
    }
    
    public async Task<Employee?> GetAsync(int employeeId) {
        const string sqlEmployee = "select employeeId, name, taxnumber, street, zipcode, civilparish, version from employees where employeeid=@employeeid";
        var state  = await _cnn.QuerySingleAsync<EmployeeData>(sqlEmployee, new { employeeId });
        if( state is null ) {
            // not found
            return null;
        }

        const string sqlContacts = "select value, contacttype from contacts where employeeid=@employeeid";
        var contacts = await _cnn.QueryAsync<ContactData>(sqlContacts, new { employeeId });

        state.Contacts = contacts?.ToList( ) ?? new List<ContactData>( );
        return new Employee(state);
    }

    private async Task<bool> UpdateEmployeeAsync(Employee employee) {
        const string sql = @"update employees set name=@name, taxnumber=@taxnumber, street=@street, zipcode=@zipcode, civilparish=@civilparish
                             output inserted.version
                             where employeeid=@employeeid and version=@version";
        var state = employee.GetInternalState( );
        var events = employee.Events;
        var newVersion = await _cnn.QuerySingleAsync<byte[]>(sql, state);
        if( newVersion is null ) {
            // could also return false
            return false;
        }

        state.Version = newVersion;

        return await SaveContactsAsync(state) && await SaveEventsAsync(employee);

    }


    private async Task<bool> InsertEmployeeAsync(Employee employee) {
        const string sql = @"insert into employees(name, taxnumber, street, zipcode, civilparish) 
                             output inserted.employeeid, inserted.version  
                             values(@name, @taxnumber, @street, @zipcode, @civilparish)";

        var state = employee.GetInternalState( );
        var inserted = await _cnn.QueryFirstAsync<Info>(sql, state);

        if( inserted is null ) {
            return false;
            // could also throw instead of returning false
        }

        state.EmployeeId = inserted.EmployeeId;
        state.Version = inserted.Version;

        return await SaveContactsAsync(state) && await SaveEventsAsync(employee);
    }

    private async Task<bool> SaveContactsAsync(EmployeeData state) {
        const string sqlDelete = "delete from contacts where employeeid = @employeeid";
        const string sqlInsert = "insert into contacts(value, contacttype, employeeid) values(@value, @contacttype, @employeeid)";

        await _cnn.ExecuteAsync(sqlDelete, new { state.EmployeeId });
        var total = await _cnn.ExecuteAsync(sqlInsert,
                                            state.Contacts.Select(c => new {
                                                                               c.Value,
                                                                               ContactType = (int)c.ContactType,
                                                                               state.EmployeeId
                                                                           }));

        return total == state.Contacts.Count;
    }
    
    
    private async Task<bool> SaveEventsAsync(Employee employee) {
        var events = employee.Events.ToArray( );
        if( events.Length == 0 ) {
            return true;
        }

        const string sql = "insert into domainevents (event, eventtype, employeeid) values(@Event, @Eventtype, @id)";

        var lst = events.Select(async evt => new {
                                                     Event = await _serializer.Serialize(evt),
                                                     EventType = evt.GetType( ).FullName,
                                                     employee.Id
                                                 })
                        .Select(t => t.Result);
        var savedCount = await _cnn.ExecuteAsync(sql,lst);

        if( savedCount != events.Length ) {
            return false;
        }

        employee.ClearEvents(  );
        return true;

    }


    class Info {
        public int EmployeeId { get; set; }

        public byte[] Version { get; set; } = Array.Empty<byte>( );
    }
}
