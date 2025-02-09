namespace InnoClinic.Appointments.Core.Models
{
    public class DoctorModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
        public Guid AccountId { get; set; }
    }
}
