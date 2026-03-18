using EmployeeAPIUI.AutoMapper;
using EmployeeAPI.Domain.Entities;
using EmployeeAPIUI.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace EmployeeAPIUI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity -> ViewModel
            CreateMap<Employee, EmployeeVM>();

            // ViewModel -> Entity
            CreateMap<EmployeePostVM, Employee>();
        }

    }
}

