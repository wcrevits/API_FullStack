using EmployeeAPI.Domain.Entities;

namespace EmployeeAPI.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee?> GetById(int id);
        Task Add(Employee employee);
        Task Update(Employee employee);
        Task Delete(int id);
    }
}