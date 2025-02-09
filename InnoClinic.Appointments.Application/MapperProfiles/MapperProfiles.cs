using AutoMapper;
using InnoClinic.Appointments.Core.Dto;
using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.Application.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<DoctorDto, DoctorModel>();
            CreateMap<DoctorModel, DoctorDto>();

            CreateMap<MedicalServiceDto, MedicalServiceModel>();

            CreateMap<PatientDto, PatientModel>();
        }
    }
}
