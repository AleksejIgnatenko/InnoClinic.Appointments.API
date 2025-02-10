using InnoClinic.Appointments.API.Contracts;
using InnoClinic.Appointments.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Appointments.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("appointments-by-doctor")]
        public async Task<ActionResult> GetAppointmentsByDoctorAsync()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            return Ok(await _appointmentService.GetAppointmentsByDoctorAsync(token));
        }

        [Authorize]
        [HttpGet("appointments-by-doctor-and-date")]
        public async Task<ActionResult> GetAppointmentsByDoctorAndDateAsync(string date)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            return Ok(await _appointmentService.GetAppointmentsByDoctorAndDateAsync(token, date));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAppointmentAsync(Guid id, [FromBody] AppointmentRequest appointmentRequest)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _appointmentService.UpdateAppointmentAsync(id, token, appointmentRequest.DoctorId,
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
