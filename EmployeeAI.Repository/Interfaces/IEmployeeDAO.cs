using EmployeeAPI.Domain.Entities;

namespace EmployeeAPI.Repository.Interfaces
{
    public interface IEmployeeDAO
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}