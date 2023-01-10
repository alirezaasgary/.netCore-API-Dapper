using Employee.DTOs;
using Employee.Entities;
using Employee.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }


        [HttpGet]
        public async Task<ActionResult> GetCompanies()
        {
            try
            {
                var companies = await _companyRepo.GetCompanies();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> CompanyById(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyById(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var newCompany = await _companyRepo.CreateCompany2(company);
                return CreatedAtAction("CompanyById", new { Id = newCompany.Id }, newCompany);
                //   return CreatedAtRoute("CompanyById", new { id = newCompany.Id }, newCompany);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var oldCompany = await _companyRepo.GetCompanyById(id);

                if (oldCompany == null)
                {
                    return NotFound();
                }
                await _companyRepo.UpdateCompany(company, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                var company= await _companyRepo.GetCompanyById(id);
                if(company==null) return NotFound();

               await _companyRepo.DeleteCompany(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

       
        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyForEmployee(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyByEmployeeId(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetCompanyEmployeesMultipleResult(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyEmployeesMultipleResults(id);
                if (company == null)
                    return NotFound();

                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
