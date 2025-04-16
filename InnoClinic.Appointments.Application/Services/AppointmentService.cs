using InnoClinic.Appointments.Core.Exceptions;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
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
        private readonly IAppointmentResultRepository _appointmentResultRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository, IMedicalServiceRepository medicalServiceRepository, IValidationService validationService, IJwtTokenService jwtTokenService, IAppointmentResultRepository appointmentResultRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _medicalServiceRepository = medicalServiceRepository;
            _validationService = validationService;
            _jwtTokenService = jwtTokenService;
            _appointmentResultRepository = appointmentResultRepository;
        }

        public async Task CreateAppointmentAsync(Guid patientId, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            var medicalService = await _medicalServiceRepository.GetByIdAsync(medicalServiceId);

            var appointment = new AppointmentEntity
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

        public async Task CreateAppointmentAsync(string token, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);

            var patient = await _patientRepository.GetByAccountIdAsync(accountId);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            var medicalService = await _medicalServiceRepository.GetByIdAsync(medicalServiceId);

            var appointment = new AppointmentEntity
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

        public async Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppointmentEntity>> GetDoctorAppointmentsByAccessTokenAsync(string token)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);
            return await _appointmentRepository.GetAllByDoctorAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDateAsync(string date)
        {
            var appointments = await _appointmentRepository.GetByDateAsync(date);

            appointments = appointments.OrderBy(a => ParseStartTime(a.Time))
                                       .ThenBy(a => a.Doctor.FirstName)
                                       .ThenBy(a => a.Doctor.LastName)
                                       .ThenBy(a => a.MedicalService);

            return appointments;
        }

        public async Task<IEnumerable<AppointmentEntity>> GetDoctorAppointmentsByAccessTokenAndDateAsync(string token, string date)
        {
            var accountId = _jwtTokenService.GetAccountIdFromAccessToken(token);
            return await _appointmentRepository.GetByAccountIdAndDateAsync(accountId, date);
        }

        public async Task<List<string>> GetAllAvailableTimeSlotsAsync(string date, int timeSlotSize, Guid doctorId)
        {
            TimeSpan startWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);

            var freeTimes = new List<string>();
            var lastEndTime = startWorkingTime;

            var appointments = await _appointmentRepository.GetByDateAndDoctorIdAsync(date, doctorId);
            appointments = appointments.OrderBy(a => DateTime.Parse(a.Time.Split(" - ")[0])).ToList();

            foreach (var appointment in appointments)
            {
                var startTime = DateTime.Parse(appointment.Time.Split(" - ")[0]);
                var endTime = DateTime.Parse(appointment.Time.Split(" - ")[1]); 

                while (lastEndTime.Add(TimeSpan.FromMinutes(timeSlotSize)) <= startTime.TimeOfDay)
                {
                    var freeStart = lastEndTime;

                    if (freeStart.Add(TimeSpan.FromMinutes(timeSlotSize)) <= endWorkingTime)
                    {
                        var freeEnd = freeStart.Add(TimeSpan.FromMinutes(timeSlotSize));
                        freeTimes.Add($"{freeStart:hh\\:mm} - {freeEnd:hh\\:mm}");
                    }

                    lastEndTime = freeStart.Add(TimeSpan.FromMinutes(timeSlotSize));
                }

                lastEndTime = endTime.TimeOfDay; 
            }

            while (lastEndTime.Add(TimeSpan.FromMinutes(timeSlotSize)) <= endWorkingTime)
            {
                var freeStart = lastEndTime;
                var freeEnd = freeStart.Add(TimeSpan.FromMinutes(timeSlotSize));
                freeTimes.Add($"{freeStart:hh\\:mm} - {freeEnd:hh\\:mm}");
                lastEndTime = freeEnd;
            }

            return freeTimes;
        }

        public async Task<IEnumerable<AppointmentEntity>> GetAllAppointmentsByPatientIdAsync(Guid patientId)
        {
            return await _appointmentRepository.GetAllByPatientIdAsync(patientId);
        }

        public async Task<bool> IsAppointmentResultsExistenceAsync(Guid id)
        {
            return await _appointmentResultRepository.IsAppointmentResultsExistenceAsync(id);
        }

        public async Task UpdateAppointmentAsync(Guid id, Guid doctorId, Guid medicalServiceId, string date, string time, bool isApproved)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            var medicalService = await _medicalServiceRepository.GetByIdAsync(medicalServiceId);

            appointment.Doctor = doctor;
            appointment.MedicalService = medicalService;
            appointment.Date = date;
            appointment.Time = time;
            appointment.IsApproved = isApproved;

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

        private TimeSpan ParseStartTime(string time)
        {
            string[] timeParts = time.Split('-');

            if (TimeSpan.TryParse(timeParts[0].Trim(), out TimeSpan startTime))
            {
                return startTime;
            }

            throw new ArgumentException("Invalid time format: " + time);
        }
    }
}
