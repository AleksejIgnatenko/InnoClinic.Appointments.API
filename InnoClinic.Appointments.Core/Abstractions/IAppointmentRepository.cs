﻿using InnoClinic.Appointments.Core.Models;

namespace InnoClinic.Appointments.DataAccess.Repositories
{
    public interface IAppointmentRepository : IRepositoryBase<AppointmentModel>
    {
        Task<IEnumerable<AppointmentModel>> GetAllAsync();
        Task<AppointmentModel> GetByIdAsync(Guid id);
        Task<IEnumerable<AppointmentModel>> GetByAccountIdAsync(Guid accountId);
        Task<IEnumerable<AppointmentModel>> GetByDateAsync(string date);
        Task<IEnumerable<AppointmentModel>> GetByAccountIdAndDateAsync(Guid accountId, string date);
    }
}