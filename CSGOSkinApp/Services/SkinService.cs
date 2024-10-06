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
            string[] substrings = nameSubstring.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            substrings = substrings.Select(s => s.Trim().ToLower()).ToArray();
            if(!nameSubstring.Contains("stat"))
            {
                var groupedSkins = await _context.Skins
                    .Where(skin => substrings.All(substring => skin.Name.ToLower().Contains(substring)) && !skin.Name.ToLower().Contains("stattrak"))
                    .GroupBy(skin => skin.Name)
                    .Select(group => new
                    {
                        SkinName = group.Key,
                        Skins = group.GroupBy(s => s.Weapon)
                                    .FirstOrDefault()
                                    .Select(s => s)
                    })
                    .ToListAsync();

                return groupedSkins.SelectMany(g => g.Skins).ToList();
            }
            else 
            {
                var groupedSkins = await _context.Skins
                    .Where(skin => substrings.All(substring => skin.Name.ToLower().Contains(substring)) && skin.Name.ToLower().Contains("stattrak"))
                    .GroupBy(skin => skin.Name)
                    .Select(group => new
                    {
                        SkinName = group.Key,
                        Skins = group.GroupBy(s => s.Weapon)
                                    .FirstOrDefault()
                                    .Select(s => s)
                    })
                    .ToListAsync();

                return groupedSkins.SelectMany(g => g.Skins).ToList();
            }
        }
    }
}