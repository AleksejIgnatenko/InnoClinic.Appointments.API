namespace InnoClinic.Appointments.Core.Models.MedicalServiceModels
{
    public class MedicalServiceDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
