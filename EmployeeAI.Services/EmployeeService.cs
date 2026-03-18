using EmployeeAPI.Domain.Entities;
using EmployeeAPI.Repository.Interfaces;
using EmployeeAPI.Services.Interfaces;

namespace EmployeeAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDAO _employeeDAO;

        public EmployeeService(IEmployeeDAO employeeDAO)
        {
            _employeeDAO = employeeDAO;
        }

        // Alle employees ophalen
        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _employeeDAO.GetAllAsync();
        }

        // Employee ophalen via ID
        public async Task<Employee?> GetById(int id)
        {
            return await _employeeDAO.GetByIdAsync(id);
        }

        // Nieuwe employee toevoegen
        public async Task Add(Employee employee)
        {
            await _employeeDAO.AddAsync(employee);
        }

        // Employee aanpassen
        public async Task Update(Employee employee)
        {
            await _employeeDAO.UpdateAsync(employee);
        }

        // Employee verwijderen
        public async Task Delete(int id)
        {
            await _employeeDAO.DeleteAsync(id);
        }
    }
}