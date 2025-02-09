namespace InnoClinic.Appointments.Core.Dto
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
        public Guid AccountId { get; set; }
    }
}
