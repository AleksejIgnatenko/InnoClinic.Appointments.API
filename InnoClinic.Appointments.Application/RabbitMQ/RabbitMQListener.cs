using System.Text;
using AutoMapper;
using InnoClinic.Appointments.Core.Models.DoctorModels;
using InnoClinic.Appointments.Core.Models.MedicalServiceModels;
using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Appointments.DataAccess.Repositories;
using InnoClinic.Appointments.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InnoClinic.Appointments.Application.RabbitMQ
{
    public class RabbitMQListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly IMapper _mapper;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalServiceRepository _mediicalServiceRepository;
        private readonly IPatientRepository _patientRepository;

        public RabbitMQListener(IOptions<RabbitMQSetting> rabbitMqSetting, IDoctorRepository doctorRepository, IMapper mapper, IMedicalServiceRepository mediicalServiceRepository, IPatientRepository patientRepository)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _mediicalServiceRepository = mediicalServiceRepository;
            _patientRepository = patientRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region doctor
            var addDoctorConsumer = new EventingBasicConsumer(_channel);
            addDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);
                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorRepository.CreateAsync(doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_DOCTOR_IN_APPOINTMENTS_QUEUE, false, addDoctorConsumer);

            var updateDoctorConsumer = new EventingBasicConsumer(_channel);
            updateDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);

                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorRepository.UpdateAsync(doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_DOCTOR_IN_APPOINTMENTS_QUEUE, false, updateDoctorConsumer);

            var deleteDoctorConsumer = new EventingBasicConsumer(_channel);
            deleteDoctorConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var doctorDto = JsonConvert.DeserializeObject<DoctorDto>(content);
                var doctor = _mapper.Map<DoctorEntity>(doctorDto);

                await _doctorRepository.DeleteAsync(doctor);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.DELETE_DOCTOR_IN_APPOINTMENTS_QUEUE, false, deleteDoctorConsumer);
            #endregion

            #region medicalService
            var addMedicalServiceConsumer = new EventingBasicConsumer(_channel);
            addMedicalServiceConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var medicalServiceDto = JsonConvert.DeserializeObject<MedicalServiceDto>(content);
                var medicalService = _mapper.Map<MedicalServiceEntity>(medicalServiceDto);

                await _mediicalServiceRepository.CreateAsync(medicalService);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_MEDICAL_SERVICE_QUEUE, false, addMedicalServiceConsumer);

            var updateMedicalServiceConsumer = new EventingBasicConsumer(_channel);
            updateMedicalServiceConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var medicalServiceDto = JsonConvert.DeserializeObject<MedicalServiceDto>(content);
                var medicalService = _mapper.Map<MedicalServiceEntity>(medicalServiceDto);

                await _mediicalServiceRepository.UpdateAsync(medicalService);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_MEDICAL_SERVICE_QUEUE, false, updateMedicalServiceConsumer);

            var deleteMedicalServiceConsumer = new EventingBasicConsumer(_channel);
            deleteMedicalServiceConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var medicalServiceDto = JsonConvert.DeserializeObject<MedicalServiceDto>(content);
                var medicalService = _mapper.Map<MedicalServiceEntity>(medicalServiceDto);

                await _mediicalServiceRepository.DeleteAsync(medicalService);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.DELETE_MEDICAL_SERVICE_QUEUE, false, deleteMedicalServiceConsumer);
            #endregion

            #region patient
            var addPatientConsumer = new EventingBasicConsumer(_channel);
            addPatientConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var patientDto = JsonConvert.DeserializeObject<PatientDto>(content);
                var patient = _mapper.Map<PatientEntity>(patientDto);

                await _patientRepository.CreateAsync(patient);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.ADD_PATIENT_IN_APPOINTMENTS_QUEUE, false, addPatientConsumer);

            var updatePatientConsumer = new EventingBasicConsumer(_channel);
            updatePatientConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var patientDto = JsonConvert.DeserializeObject<PatientDto>(content);
                var patient = _mapper.Map<PatientEntity>(patientDto);

                await _patientRepository.UpdateAsync(patient);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.UPDATE_PATIEN_IN_APPOINTMENTST_QUEUE, false, updatePatientConsumer);

            var deletePatientConsumer = new EventingBasicConsumer(_channel);
            deletePatientConsumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                var patientDto = JsonConvert.DeserializeObject<PatientDto>(content);
                var patient = _mapper.Map<PatientEntity>(patientDto);

                await _patientRepository.DeleteAsync(patient);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(RabbitMQQueues.DELETE_PATIENT_IN_APPOINTMENTS_QUEUE, false, deletePatientConsumer);
            #endregion


            return Task.CompletedTask;
        }
    }
}
