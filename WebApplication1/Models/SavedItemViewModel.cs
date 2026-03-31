namespace WebApplication1.Models
{
    // BUG FIX: Created missing ViewModel for Saved.cshtml
    public class SavedItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
