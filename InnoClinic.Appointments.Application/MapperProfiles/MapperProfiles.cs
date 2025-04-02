using AutoMapper;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Appointments.Application.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<DoctorDto, DoctorEntity>();
            CreateMap<DoctorEntity, DoctorDto>();

            CreateMap<MedicalServiceDto, MedicalServiceEntity>();

            CreateMap<PatientDto, PatientEntity>();
        }
    }
}
