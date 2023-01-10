using Employee.DTOs;
using Employee.Entities;

namespace Employee.Repository
{
    public interface ICompanyRepository
    {

        public Task<IEnumerable<Company>> GetCompanies();
        public Task<Company> GetCompanyById(int id);
        public Task CreateCompany(CompanyForCreationDto company);
        public Task<Company> CreateCompany2(CompanyForCreationDto company);
        public Task UpdateCompany(CompanyForUpdateDto company, int id);
        public Task DeleteCompany(int id);

        public Task<Company> GetCompanyByEmployeeId(int id);
        public Task<Company> GetCompanyEmployeesMultipleResults(int id);

    }
}
