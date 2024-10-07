using CSGOSkinApp.Data;
using CSGOSkinApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOSkinApp.Services
{
    public class SkinService : ISkinService
    {
        private readonly AppDbContext _context;
        public SkinService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Skin?>> GetAllViaNameSubstring(string nameSubstring)
        {
            string[] substrings = nameSubstring.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                               .Select(s => s.Trim().ToLower())
                                               .ToArray();

            bool includeStatTrak = nameSubstring.Contains("stat", StringComparison.OrdinalIgnoreCase);

            // Initial query to filter skins
            var query = _context.Skins.AsNoTracking()
                .Where(skin => substrings.All(substring => skin.Name.ToLower().Contains(substring)));

            // Apply StatTrak filter
            if (includeStatTrak)
            {
                query = query.Where(skin => skin.Name.ToLower().Contains("stattrak"));
            }
            else
            {
                query = query.Where(skin => !skin.Name.ToLower().Contains("stattrak"));
            }

            // Execute the query and group by name
            var groupedSkins = await query
                .GroupBy(skin => skin.Name)
                .Select(group => new{
                    SkinName = group.Key,
                    Skins = group.ToList()
                })
                .FirstOrDefaultAsync();

            return groupedSkins?.Skins ?? new List<Skin>();
        }
    }
}