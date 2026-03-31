using WebApplication1.Models;

namespace WebApplication1.Data;

public interface IMuseumData
{
    IReadOnlyList<OpeningHour> GetOpeningHours();
    IReadOnlyList<FAQ> GetActiveFaqs();
}