using EmployeeAPI.Domain.Data;
using EmployeeAPI.Domain.Entities;
using EmployeeAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Repository
{
    public class EmployeeDAO : IEmployeeDAO
    {
        private readonly SwaggerDbContext _context;

        public EmployeeDAO(SwaggerDbContext context)
        {
            _context = context;
        }

        // Haalt alle employees op uit de database
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        // Haalt één employee op via het ID
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        // Nieuwe employee toevoegen
        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        // Employee aanpassen
        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        // Employee verwijderen
        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}