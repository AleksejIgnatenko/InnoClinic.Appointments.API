using InnoClinic.Appointments.Application.Validators;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Services
{
    public class ValidationService : IValidationService
    {
        public Dictionary<string, string> Validation(AppointmentEntity appointmentModel)
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

        public Dictionary<string, string> Validation(AppointmentResultEntity appointmentResultModel)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            AppointmentResultValidator validations = new AppointmentResultValidator();
            FluentValidation.Results.ValidationResult validationResult = validations.Validate(appointmentResultModel);
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
