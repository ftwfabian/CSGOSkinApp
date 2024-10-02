using System;

namespace CSGOSkinApp.Entities
{
    public class Skin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Weapon {get;set;}
        public string Condition { get; set;}
        public float Float {get; set;}
        public decimal MarketPrice { get; set;}
        public string? Url { get; set;}
        public DateTime dateListed { get; set; }
        public string? sticker1 { get; set; }
        public string? sticker2 { get; set; }
        public string? sticker3 { get; set; }
        public string? sticker4 { get; set; }
        public string? sticker5 { get; set; }

    }
}