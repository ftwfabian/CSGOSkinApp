
using CSGOSkinApp.Entities;

namespace CSGOSkinApp.Services
{
    public interface ISkinService
    {
        Task<List<Skin?>> GetAllViaNameSubstring(string name);
    }
}