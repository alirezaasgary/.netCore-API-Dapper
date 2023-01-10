using Dapper;
using Employee.Context;
using Employee.DTOs;
using Employee.Entities;
using System.Data;

namespace Employee.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DapperContext _context;
        public CompanyRepository( DapperContext context)
        {
            _context= context;
        }

        

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            string query = "select * from company";
            
            using(var connection = _context.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompanyById(int id)
        {
            string query = "select * from company where id=@Id";

            using (var connection=_context.CreateConnection() )
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(query, new {id});
                return company;
            }
        }

        public async Task CreateCompany(CompanyForCreationDto company)
        {
            var query = "insert into company(Name,Address,Country) VALUES  (@name , @address,@country)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        //Creating a Better API Solution
        public async Task<Company> CreateCompany2(CompanyForCreationDto company)
        {
            var query = "INSERT INTO company (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdCompany = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public async Task UpdateCompany(CompanyForUpdateDto company,int id)
        {
            var query = "update company set name=@name,address=@address,country=@country where id=@id";

            var parameters = new DynamicParameters();
            parameters.Add("id", id, DbType.Int32);
            parameters.Add("name", company.Name, DbType.String);
            parameters.Add("address" , company.Address, DbType.String);
            parameters.Add("country",company.Country, DbType.String);   

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCompany(int id)
        {
            string query = "delete from company where id=@id";
            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new {id});
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(int id)
        {
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

                return company;
            }
        }

        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            var query = "SELECT * FROM Company WHERE Id = @Id;" +
              "SELECT * FROM Employee WHERE CompanyId = @Id";

            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                    company.Employees = (await multi.ReadAsync<Employee.Entities.Employee>()).ToList();

                return company;
            }
        }


    }
}
