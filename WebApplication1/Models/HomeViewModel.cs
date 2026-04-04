using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class HomeViewModel
    {
        public List<OpeningHour> OpeningHours { get; set; } = [];
        public List<FAQ> FAQs { get; set; } = [];
    }
}