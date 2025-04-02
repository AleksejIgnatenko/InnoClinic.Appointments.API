using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using InnoClinic.Appointments.DataAccess.Repositories;

namespace InnoClinic.Appointments.Application.Services
{
    public class AppointmentResultService : IAppointmentResultService
    {
        private readonly IAppointmentResultRepository _appointmentResultRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IValidationService _validationService;

        public AppointmentResultService(IAppointmentResultRepository appointmentResultRepository, IAppointmentRepository appointmentRepository, IValidationService validationService)
        {
            _appointmentResultRepository = appointmentResultRepository;
            _appointmentRepository = appointmentRepository;
            _validationService = validationService;
        }

        public async Task CreateAppointmentResultAsync(string complaints, string conclusion, string recomendations, string diagnisis, Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            var appointmentResult = new AppointmentResultEntity
            {
                Id = Guid.NewGuid(),
                Complaints = complaints,
                Conclusion = conclusion,
                Recomendations = recomendations,
                Diagnisis = diagnisis,
                Appointment = appointment,
            };

            var validationErrors = _validationService.Validation(appointment);

            if (validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            await _appointmentResultRepository.CreateAsync(appointmentResult);
        }

        public async Task<IEnumerable<AppointmentResultEntity>> GetAllAppointmentResultsAsync()
        {
            return await _appointmentResultRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppointmentResultEntity>> GetAllAppointmentResultsByAppointmentIdAsync(Guid appointmentId)
        {
            return await _appointmentResultRepository.GetAllByAppointmentIdAsync(appointmentId);
        }

        public async Task UpdateAppointmentResultAsync(Guid id, string complaints, string conclusion, string recomendations, string diagnisis, Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

            var appointmentResult = new AppointmentResultEntity
            {
                Id = id,
                Complaints = complaints,
                Conclusion = conclusion,
                Recomendations = recomendations,
                Diagnisis = diagnisis,
                Appointment = appointment,
            };

            var validationErrors = _validationService.Validation(appointment);

            if (validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            await _appointmentResultRepository.UpdateAsync(appointmentResult);
        }

        public async Task DeleteAppointmentResultAsync(Guid id)
        {
            var appointmentResult = await _appointmentResultRepository.GetByIdAsync(id);
            await _appointmentResultRepository.DeleteAsync(appointmentResult);
        }
    }
}
