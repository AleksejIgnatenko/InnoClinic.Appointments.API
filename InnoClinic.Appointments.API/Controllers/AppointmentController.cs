using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace InnoClinic.Appointments.API.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Receptionist")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateReceptionistAppointmentAsync([FromBody] CreateReceptionistAppointmentRequest createReceptionistAppointmentRequest)
        {
            await _appointmentService.CreateAppointmentAsync(createReceptionistAppointmentRequest.PatientId, createReceptionistAppointmentRequest.DoctorId, createReceptionistAppointmentRequest.MedicalServiceId, createReceptionistAppointmentRequest.Date, createReceptionistAppointmentRequest.Time, createReceptionistAppointmentRequest.IsApproved);

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult> CreateAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _appointmentService.CreateAppointmentAsync(token, appointmentRequest.DoctorId,
                appointmentRequest.MedicalServiceId, appointmentRequest.Date, appointmentRequest.Time, appointmentRequest.IsApproved);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAppointmentsAsync()
        {
            return Ok(await _appointmentService.GetAllAppointmentsAsync());
        }

        [HttpGet("appointments-by-doctor")]
        public async Task<ActionResult> GetAppointmentsByDoctorAsync()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _appointmentService.GetDoctorAppointmentsByAccessTokenAsync(token));
        }

        [HttpGet("appointments-by-date")]
        public async Task<ActionResult> GetAppointmentsByDateAsync(string date)
        {
            return Ok(await _appointmentService.GetAppointmentsByDateAsync(date));
        }

        [HttpGet("appointments-by-doctor-and-date")]
        public async Task<ActionResult> GetAppointmentsByDoctorAndDateAsync(string date)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _appointmentService.GetDoctorAppointmentsByAccessTokenAndDateAsync(token, date));
        }

        [AllowAnonymous]
        [HttpGet("all-available-time-slots")]
        public async Task<ActionResult> GetAllAvailableTimeSlotsAsync(string date, int timeSlotSize, Guid doctorId)
        {
            return Ok(await _appointmentService.GetAllAvailableTimeSlotsAsync(date, timeSlotSize, doctorId));
        }

        [HttpGet("by-patient-id/{patientId:guid}")]
        public async Task<ActionResult> GetAllAppointmentsByPatientIdAsync(Guid patientId)
        {
            return Ok(await _appointmentService.GetAllAppointmentsByPatientIdAsync(patientId));
        }

        [HttpGet("is-appointment-results-existence/{id:guid}")]
        public async Task<ActionResult<bool>> IsAppointmentResultsExistenceAsync(Guid id)
        {
            return Ok(await _appointmentService.IsAppointmentResultsExistenceAsync(id));
        }

        [Authorize(Roles = "Receptionist, Patient")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAppointmentAsync(Guid id, [FromBody] AppointmentRequest appointmentRequest)
        {
            await _appointmentService.UpdateAppointmentAsync(id, appointmentRequest.DoctorId,
                appointmentRequest.MedicalServiceId, appointmentRequest.Date, appointmentRequest.Time, appointmentRequest.IsApproved);

            return Ok();
        }

        [Authorize(Roles = "Receptionist")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAppointmentAsync(Guid id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok();
        }
    }
}
