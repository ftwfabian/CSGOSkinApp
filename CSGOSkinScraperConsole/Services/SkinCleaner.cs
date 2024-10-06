using CSGOSkinApp.Entities;
using CSGOSkinApp.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CSGOSkinApp.Services
{
public class SkinCleaner
{
    private readonly AppDbContext _context;
    public SkinCleaner(AppDbContext context)
    {
        _context = context;
    }

    public async Task CleanWeaponAndConditionOffOfNameAsync()
    {
        return;
    }
}
}