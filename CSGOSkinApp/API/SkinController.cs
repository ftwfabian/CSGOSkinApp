using CSGOSkinApp.Data;
using CSGOSkinApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSGOSkinApp.Services;

namespace CSGOSkinApp.API
{
    [ApiController]
    [Route("api/Skins")]
    public class SkinController: ControllerBase
    {
        private readonly AppDbContext _context;
        private ISkinService _skinService;
        public SkinController(AppDbContext context, ISkinService skinService)
        {
            _context = context;
            _skinService = skinService;
        }

        [HttpGet("{nameSubstring}")]
        public async Task<ActionResult> GetAllViaNameSubstring(string nameSubstring)
        {
            List<Skin> skins = await _skinService.GetAllViaNameSubstring(nameSubstring);

            if(skins.Any())
            {
                var response = new
                {
                    Success = true,
                    Message = "Skins found",
                    Data = skins,
                    Count = skins.Count
                };

                return Ok(response);
            }

            var errorResponse = new
            {
                Success = false,
                Message = "No skins found for the given name",
                Data = new List<Skin>(),
                Count = 0
            };

            return BadRequest(errorResponse);
        }

    }
}