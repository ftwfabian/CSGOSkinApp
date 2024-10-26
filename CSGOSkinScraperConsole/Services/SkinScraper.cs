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
    public class SkinScraperDMarket
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly AppDbContext _context;
        private readonly ILogger<SkinScraperDMarket> _logger;

        public SkinScraperDMarket(AppDbContext context, ILogger<SkinScraperDMarket> logger)
        {
            _context = context;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://api.dmarket.com");
        }

        public async Task ScrapeSkins(string title)
        {
            string? cursor = null;
            int totalScraped = 0;
            do
            {
                try
                {
                    string endpoint = $"exchange/v1/market/items?gameId=a8db&title={title}&limit=100&orderBy=updated&orderDir=desc&currency=USD";
                    if (cursor != null)
                    {
                        endpoint += $"&cursor={cursor}";
                    }

                    HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();

                    var (skins, nextCursor) = ParseJsonToSkins(data);

                    await _context.Skins.AddRangeAsync(skins);
                    await _context.SaveChangesAsync();

                    totalScraped += skins.Count;
                    _logger.LogInformation(cursor);
                    _logger.LogInformation($"Scraped {skins.Count} skins. Total: {totalScraped}");

                    cursor = nextCursor;
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Error fetching data from DMarket API: {e.Message}");
                    break;
                }
                catch (JsonException e)
                {
                    _logger.LogError($"Error parsing JSON data: {e.Message}");
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Unexpected error occurred: {e.Message}");
                    break;
                }
            } while (cursor != null);

            _logger.LogInformation($"Scraping completed. Total skins scraped: {totalScraped}");
        }

        public async Task ScrapeStickers()
        {
            string? cursor = null;
            int totalScraped = 0;
            do
            {
                try
                {
                    string endpoint = $"exchange/v1/market/items?gameId=a8db&title=sticker&limit=100&orderBy=updated&orderDir=desc&currency=USD";
                    if (cursor != null)
                    {
                        endpoint += $"&cursor={cursor}";
                    }

                    HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    string data = await response.Content.ReadAsStringAsync();

                    var (skins, nextCursor) = ParseJsonToSkins(data);

                    await _context.Skins.AddRangeAsync(skins);
                    await _context.SaveChangesAsync();

                    totalScraped += skins.Count;
                    _logger.LogInformation(cursor);
                    _logger.LogInformation($"Scraped {skins.Count} skins. Total: {totalScraped}");

                    cursor = nextCursor;
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"Error fetching data from DMarket API: {e.Message}");
                    break;
                }
                catch (JsonException e)
                {
                    _logger.LogError($"Error parsing JSON data: {e.Message}");
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Unexpected error occurred: {e.Message}");
                    break;
                }
            } while (cursor != null);

            _logger.LogInformation($"Scraping completed. Total skins scraped: {totalScraped}");
        }

        public async Task CleanseScrapingOfNonGunsAndNonStickers()
        {
            List<Skin> recordsToRemove = await _context.Skins
                .Where(x => x.Condition == "Unknown" && !x.Name.Contains("Sticker"))
                .ToListAsync();

            _context.Skins.RemoveRange(recordsToRemove);
            await _context.SaveChangesAsync();
        }

        private (List<Skin> skins, string? nextCursor) ParseJsonToSkins(string json)
        {
            List<Skin> skins = new List<Skin>();
            string? nextCursor = null;

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            JsonElement objects = root.GetProperty("objects");

            foreach (JsonElement skinData in objects.EnumerateArray())
            {
                try
                {
                    Skin skin = new Skin
                    {
                        Name = GetStringProperty(skinData, "title"),
                        Weapon = GetStringProperty(skinData, "title").Split('|')[0].Trim(),
                        Condition = ExtractCondition(GetStringProperty(skinData, "title")),
                        Float = GetFloatProperty(skinData.GetProperty("extra"), "floatValue"),
                        MarketPrice = GetDecimalProperty(skinData.GetProperty("price"), "USD") / 100m, // Price is in cents
                        Url = GetStringProperty(skinData, "image"),
                        dateListed = DateTimeOffset.FromUnixTimeSeconds(GetLongProperty(skinData, "createdAt")).DateTime
                    };

                    // Extract stickers
                    if (skinData.GetProperty("extra").TryGetProperty("stickers", out JsonElement stickers))
                    {
                        for (int i = 0; i < Math.Min(stickers.GetArrayLength(), 5); i++)
                        {
                            string stickerName = GetStringProperty(stickers[i], "name");
                            switch (i)
                            {
                                case 0: skin.sticker1 = stickerName; break;
                                case 1: skin.sticker2 = stickerName; break;
                                case 2: skin.sticker3 = stickerName; break;
                                case 3: skin.sticker4 = stickerName; break;
                                case 4: skin.sticker5 = stickerName; break;
                            }
                        }
                    }
                    if(!skin.Condition.Equals("Unknown"))
                    {
                        skins.Add(skin);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Error parsing individual skin data: {e.Message}");
                }
            }

            if (root.TryGetProperty("cursor", out JsonElement cursorElement))
            {
                nextCursor = cursorElement.GetString();
            }

            return (skins, nextCursor);
        }

        private static string ExtractCondition(string title)
        {
            string[] conditions = { "Factory New", "Minimal Wear", "Field-Tested", "Well-Worn", "Battle-Scarred" };
            return conditions.FirstOrDefault(c => title.Contains(c)) ?? "Unknown";
        }

        private static string GetStringProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement property) ? property.GetString() ?? string.Empty : string.Empty;
        }

        private static float GetFloatProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement property) && property.TryGetSingle(out float value) ? value : 0f;
        }

        private static decimal GetDecimalProperty(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out JsonElement property))
            {
                if (property.ValueKind == JsonValueKind.String && decimal.TryParse(property.GetString(), out decimal result))
                {
                    return result;
                }
                if (property.TryGetDecimal(out decimal value))
                {
                    return value;
                }
            }
            return 0m;
        }

        private static long GetLongProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement property) && property.TryGetInt64(out long value) ? value : 0L;
        }
    }
}