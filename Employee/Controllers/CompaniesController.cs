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

    }
}
