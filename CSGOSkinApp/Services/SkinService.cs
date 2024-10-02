using CSGOSkinApp.Data;
using CSGOSkinApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSGOSkinApp.Services
{
    public class SkinService: ISkinService
    {
        private readonly AppDbContext _context;
        public SkinService(AppDbContext context)
        {   
            _context = context;
        }
        public async Task<List<Skin>> GetAllViaNameSubstring(string nameSubstring)
        {
            return await _context.Skins
                .Where(x => x.Name.Contains(nameSubstring))
                .ToListAsync();
        }
    }
}