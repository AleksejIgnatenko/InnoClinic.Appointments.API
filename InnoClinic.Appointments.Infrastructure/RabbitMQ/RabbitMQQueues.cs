namespace InnoClinic.Appointments.Infrastructure.RabbitMQ
{
    public class RabbitMQQueues
    {
        public const string ADD_DOCTOR_QUEUE = "ADD_DOCTOR_QUEUE";
        public const string UPDATE_DOCTOR_QUEUE = "UPDATE_DOCTOR_QUEUE";
        public const string DELETE_DOCTOR_QUEUE = "DELETE_DOCTOR_QUEUE";

        public const string ADD_MEDICAL_SERVICE_QUEUE = "ADD_MEDICAL_SERVICE_QUEUE";
        public const string UPDATE_MEDICAL_SERVICE_QUEUE = "UPDATE_MEDICAL_SERVICE_QUEUE";
        public const string DELETE_MEDICAL_SERVICE_QUEUE = "DELETE_MEDICAL_SERVICE_QUEUE";

        public const string ADD_PATIENT_QUEUE = "ADD_PATIENT_QUEUE";
        public const string UPDATE_PATIENT_QUEUE = "UPDATE_PATIENT_QUEUE";
        public const string DELETE_PATIENT_QUEUE = "DELETE_PATIENT_QUEUE";

    }
}
