using Homemap.ApplicationCore.Interfaces.Repositories;
using Homemap.Domain.Core;
using Homemap.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Homemap.Infrastructure.Data.Repositories
{

    internal class DeviceRepository : CrudRepository<Device>, IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Device?> FindByIdIncludingReceiverAsync(int id)
        {
            return await _context.Devices
                .Include(e => e.Receiver)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<Device>> FindAllByReceiverIdAsync(int receiverId)
        {
            return await _context.Devices
                .Where(e => e.ReceiverId == receiverId)
                .ToListAsync();
        }

        public async Task<IReadOnlyDictionary<int, Device>> FindAllByProjectIdAsync(int projectId)
        {
            return await _context.Devices
                .Include(e => e.Receiver)
                .Where(e => e.Receiver.ProjectId == projectId)
                .ToDictionaryAsync(e => e.Id);
        }
    }
}
