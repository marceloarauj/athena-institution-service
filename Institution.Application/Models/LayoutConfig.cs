using System.Text.Json.Serialization;

namespace Institution.Application.Models
{
    public class LayoutConfig
    {
        [JsonPropertyName("pageBackground")]
        public string PageBackground { get; set; } = "#ffffff";

        [JsonPropertyName("components")]
        public List<LayoutComponent> Components { get; set; } = [];

        public bool IsVisible(string type) =>
            Components.FirstOrDefault(c => c.Type == type)?.Visible ?? false;

        public string? GetContent(string type) =>
            Components.FirstOrDefault(c => c.Type == type && c.Visible)?.Content;

        public List<string> GetCustomTexts() =>
            Components
                .Where(c => c.Type == "custom_text" && c.Visible && !string.IsNullOrWhiteSpace(c.Content))
                .Select(c => c.Content!)
                .ToList();
    }

    public class LayoutComponent
    {
        [JsonPropertyName("id")]         public string Id { get; set; } = "";
        [JsonPropertyName("type")]       public string Type { get; set; } = "";
        [JsonPropertyName("visible")]    public bool Visible { get; set; } = true;
        [JsonPropertyName("color")]      public string Color { get; set; } = "#000000";
        [JsonPropertyName("content")]    public string? Content { get; set; }
        [JsonPropertyName("fontSize")]   public float FontSize { get; set; } = 14;
        [JsonPropertyName("fontWeight")] public string FontWeight { get; set; } = "normal";
    }
}
