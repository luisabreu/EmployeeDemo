using Dapper;
using Microsoft.Data.SqlClient;

namespace DemoFuncionario.Employees.Data; 

public class EmployeeRepository : IRepository<Employee> {
    private readonly SqlConnection _cnn;

    public EmployeeRepository(SqlConnection cnn) {
        _cnn = cnn;
    }

    public Task<bool> SaveAsync(Employee employee) {
        var state = employee.GetInternalState( );
        
        // save to db
        return state.EmployeeId > 0
                   ? UpdateEmployeeAsync(state)
                   : InsertEmployeeAsync(state);
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

    private async Task<bool> UpdateEmployeeAsync(EmployeeData state) {
        const string sql = @"update employees set name=@name, taxnumber=@taxnumber, street=@street, zipcode=@zipcode, civilparish=@civilparish
                             output inserted.version
                             where employeeid=@employeeid and version=@version";
        var newVersion = await _cnn.QuerySingleAsync<byte[]>(sql, state);
        if( newVersion is null ) {
            // could also return false
            return false;
        }

        state.Version = newVersion;

        return await UpdateContactsAsync(state);
    }

    private async Task<bool> InsertEmployeeAsync(EmployeeData state) {
        const string sql = @"insert into employees(name, taxnumber, street, zipcode, civilparish) 
                             output inserted.employeeid, inserted.version  
                             values(@name, @taxnumber, @street, @zipcode, @civilparish)";
        
        var inserted = await _cnn.QueryFirstAsync<Info>(sql, state);

        if( inserted is null ) {
            return false;
            // could also throw instead of returning false
        }

        state.EmployeeId = inserted.EmployeeId;
        state.Version = inserted.Version;

        return await UpdateContactsAsync(state);
    }

    private async Task<bool> UpdateContactsAsync(EmployeeData state) {
        const string sqlDelete = "delete from contacts where employeeid = @employeeid";
        const string sqlInsert = "insert into contacts(value, contacttype, employeeid) values(@value, @contacttype, @employeeid)";

        await _cnn.ExecuteAsync(sqlDelete, new { state.EmployeeId });
        var total = await _cnn.ExecuteAsync(sqlInsert,
                                            state.Contacts.Select(c => new {
                                                                               c.Value,
                                                                               ContactType = (int)c.ContactType,
                                                                               state.EmployeeId
                                                                           }));

        return total == state.Contacts.Count( );
    }


    class Info {
        public int EmployeeId { get; set; }

        public byte[] Version { get; set; } = Array.Empty<byte>( );
    }
}
