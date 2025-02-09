namespace InnoClinic.Appointments.Core.Models
{
    public class AppointmentModel
    {
        public Guid Id { get; set; }
        public PatientModel Patient { get; set; } = new PatientModel();
        public DoctorModel Doctor { get; set; } = new DoctorModel();
        public MedicalServiceModel MedicalService { get; set; } = new MedicalServiceModel();
        public string Date {  get; set; } = string.Empty;
        public string Time {  get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}
