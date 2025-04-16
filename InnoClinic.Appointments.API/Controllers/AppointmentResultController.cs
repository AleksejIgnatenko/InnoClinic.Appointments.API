using InnoClinic.Appointments.Application.Services;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace InnoClinic.Appointments.API.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentResultController : ControllerBase
    {
        private readonly IAppointmentResultService _appointmentResultService;

        public AppointmentResultController(IAppointmentResultService appointmentResultService)
        {
            _appointmentResultService = appointmentResultService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAppointmentResultAsync([FromBody] AppointmentResultRequest appointmentResultRequest)
        {
            await _appointmentResultService.CreateAppointmentResultAsync(appointmentResultRequest.Complaints, appointmentResultRequest.Conclusion, appointmentResultRequest.Recommendations, appointmentResultRequest.Diagnosis, appointmentResultRequest.AppointmentId);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAppointmentResultsAsync()
        {
            return Ok(await _appointmentResultService.GetAllAppointmentResultsAsync());
        }

        [HttpGet("by-appointment-id/{appointmentId:guid}")]
        public async Task<ActionResult> GetAllAppointmentResultsByAppointmentIdAsync(Guid appointmentId)
        {
            return Ok(await _appointmentResultService.GetAllAppointmentResultsByAppointmentIdAsync(appointmentId));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAppointmentResultAsync(Guid id, [FromBody] AppointmentResultRequest appointmentResultRequest)
        {
            await _appointmentResultService.UpdateAppointmentResultAsync(id, appointmentResultRequest.Complaints, appointmentResultRequest.Conclusion, appointmentResultRequest.Recommendations,appointmentResultRequest.Diagnosis, appointmentResultRequest.AppointmentId);

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAppointmentResultAsync(Guid id)
        {
            await _appointmentResultService.DeleteAppointmentResultAsync(id);

            return Ok();
        }
    }
}
