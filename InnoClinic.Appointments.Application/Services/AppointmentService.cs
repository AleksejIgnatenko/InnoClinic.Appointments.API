using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models;
using InnoClinic.Appointments.DataAccess.Repositories;

namespace InnoClinic.Appointments.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalServiceRepository _medicalServiceRepository;
        private readonly IValidationService _validationService;
        private readonly IJwtTokenService _jwtTokenService;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IMedicalServiceRepository medicalServiceRepository, IValidationService validationService, IJwtTokenService jwtTokenService)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _medicalServiceRepository = medicalServiceRepository;
            _validationService = validationService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task CreateAppointmentAsync(string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);

            var patient = await _patientRepository.GetByAccountIdAsync(accountId);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            var medicalService = await _medicalServiceRepository.GetByIdAsync(medicalServiceId);

            var appointment = new AppointmentModel
            {
                Id = Guid.NewGuid(),
                Patient = patient,
                Doctor = doctor,
                MedicalService = medicalService,
                Date = date,
                Time = time,
                IsApproved = isApproved
            };

            var validationErrors = _validationService.Validation(appointment);

            if (validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            await _appointmentRepository.CreateAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentModel>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorAsync(string token)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);
            return await _appointmentRepository.GetByAccountIdAsync(accountId);

        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentsByDoctorAndDateAsync(string token, string date)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);
            return await _appointmentRepository.GetByAccountIdAndDateAsync(accountId, date);

        }

        public async Task UpdateAppointmentAsync(Guid id, string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);

            var patient = await _patientRepository.GetByAccountIdAsync(accountId);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            var medicalService = await _medicalServiceRepository.GetByIdAsync(medicalServiceId);

            var appointment = new AppointmentModel
            {
                Id = id,
                Patient = patient,
                Doctor = doctor,
                MedicalService = medicalService,
                Date = date,
                Time = time,
                IsApproved = isApproved
            };

            var validationErrors = _validationService.Validation(appointment);

            if (validationErrors.Count != 0)
            {
                throw new ValidationException(validationErrors);
            }

            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            await _appointmentRepository.DeleteAsync(appointment);
        }
    }
}
