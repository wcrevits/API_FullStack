using System;
using System.Collections.Generic;

namespace EmployeeAPI.Domain.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? CompanyName { get; set; }

    public string? Designation { get; set; }

    public float Salary { get; set; }
}
