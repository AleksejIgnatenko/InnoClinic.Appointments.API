using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Appointments.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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
            return Ok(await _appointmentService.GetAppointmentsByDoctorAsync(token));
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

            return Ok(await _appointmentService.GetAppointmentsByDoctorAndDateAsync(token, date));
        }

        [HttpGet("all-available-time-slots")]
        public async Task<ActionResult> GetAllAvailableTimeSlotsAsync(string date, int timeSlotSize)
        {
            return Ok(await _appointmentService.GetAllAvailableTimeSlotsAsync(date, timeSlotSize));
        }

        [HttpGet("by-patient-id/{patientId:guid}")]
        public async Task<ActionResult> GetAllAppointmentsByPatientIdAsync(Guid patientId)
        {
            return Ok(await _appointmentService.GetAllAppointmentsByPatientIdAsync(patientId));
        }

        //role receptionist
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAppointmentAsync(Guid id, [FromBody] AppointmentRequest appointmentRequest)
        {
            await _appointmentService.UpdateAppointmentAsync(id, appointmentRequest.DoctorId,
                appointmentRequest.MedicalServiceId, appointmentRequest.Date, appointmentRequest.Time, appointmentRequest.IsApproved);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAppointmentAsync(Guid id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok();
        }
    }
}
