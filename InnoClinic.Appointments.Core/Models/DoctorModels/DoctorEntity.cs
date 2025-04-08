namespace InnoClinic.Appointments.Core.Models.DoctorModels
{
    public class DoctorEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
