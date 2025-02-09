using InnoClinic.Appointments.Application.Validators;
using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.Application.Services
{
    public class ValidationService : IValidationService
    {
        public Dictionary<string, string> Validation(AppointmentModel appointmentModel)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            AppointmentValidator validations = new AppointmentValidator();
            FluentValidation.Results.ValidationResult validationResult = validations.Validate(appointmentModel);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return errors;
        }
    }
}
