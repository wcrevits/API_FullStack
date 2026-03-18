using AutoMapper;
using EmployeeAPI.Domain.Entities;
using EmployeeAPI.ViewModels;

namespace EmployeeAPI.AutoMapper
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

