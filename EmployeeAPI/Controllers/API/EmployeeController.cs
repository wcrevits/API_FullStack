using EmployeeAPI.Domain.Entities;
using EmployeeAPI.Services.Interfaces;
using EmployeeAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the list of all Employees.
    /// </summary>
    /// <returns>The list of Employees.</returns>

    // GET: api/Employee
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeVM>>> Get()
    {
        try
        {
            // ophalen Employees via service
            var employees = await _employeeService.GetAll();

            // mapping Entity -> ViewModel
            var data = _mapper.Map<IEnumerable<EmployeeVM>>(employees);

            if (data == null || !data.Any())
            {
                // Als er geen gegevens gevonden worden
                return NotFound();
            }

            // succesvolle response
            return Ok(data);
        }
        catch (Exception ex)
        {
            // interne fout
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get one Employee.
    /// </summary>
    /// <returns>Get one Employee</returns>

    // GET: api/Employee/5
    [HttpGet("{id}", Name = "Get")]
    public async Task<ActionResult<EmployeeVM>> Get(int id)
    {
        try
        {
            // employee ophalen o.b.v. id
            var employee = await _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            // mapping Entity -> ViewModel
            var data = _mapper.Map<EmployeeVM>(employee);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Er is een interne fout opgetreden. Neem contact op met de beheerder voor assistentie."
            });
        }
    }

    /// <summary>
    /// Creates an Employee.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/Employee
    /// </remarks>

    // POST: api/Employee
    [HttpPost]
    public async Task<ActionResult<EmployeeVM>> Post([FromForm] EmployeePostVM employeePostVM)
    {
        try
        {
            // mapping ViewModel -> Entity
            var employee = _mapper.Map<Employee>(employeePostVM);

            // employee toevoegen via service
            await _employeeService.Add(employee);

            // mapping terug naar VM
            var result = _mapper.Map<EmployeeVM>(employee);

            return CreatedAtAction(nameof(Get), new { id = employee.EmployeeId }, result);
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}