using FluentValidation;
using FluentValidation.Results;
using InnoClinic.Appointments.Application.Validators;
using InnoClinic.Appointments.Core.Models.AppointmentModels;
using InnoClinic.Appointments.Core.Models.AppointmentResultModels;

namespace InnoClinic.Appointments.Application.Services
{
    public class ValidationService : IValidationService
    {
        public List<ValidationFailure> Validation(AppointmentEntity entity)
        {
            var validator = new AppointmentValidator();
            return Validate(entity, validator);
        }

        public List<ValidationFailure> Validation(AppointmentResultEntity entity)
        {
            var validator = new AppointmentResultValidator();
            return Validate(entity, validator);
        }
        private List<ValidationFailure> Validate<T>(T model, IValidator<T> validator)
        {
            var validationFailures = new List<ValidationFailure>();
            ValidationResult validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    validationFailures.Add(new ValidationFailure(failure.PropertyName, failure.ErrorMessage));
                }
            }

            return validationFailures;
        }
    }
}
