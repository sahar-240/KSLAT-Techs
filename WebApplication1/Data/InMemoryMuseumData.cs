using WebApplication1.Models;

namespace WebApplication1.Data;

public sealed class InMemoryMuseumData : IMuseumData
{
    private static readonly List<OpeningHour> OpeningHours =
    [
        new() { HourId = 1, DayOfWeek = "Monday",    IsClosed = false, OpenTime = new TimeSpan(9, 0, 0),  CloseTime = new TimeSpan(17, 0, 0) },
        new() { HourId = 2, DayOfWeek = "Tuesday",   IsClosed = false, OpenTime = new TimeSpan(9, 0, 0),  CloseTime = new TimeSpan(17, 0, 0) },
        new() { HourId = 3, DayOfWeek = "Wednesday", IsClosed = false, OpenTime = new TimeSpan(9, 0, 0),  CloseTime = new TimeSpan(17, 0, 0) },
        new() { HourId = 4, DayOfWeek = "Thursday",  IsClosed = false, OpenTime = new TimeSpan(9, 0, 0),  CloseTime = new TimeSpan(17, 0, 0) },
        new() { HourId = 5, DayOfWeek = "Friday",    IsClosed = false, OpenTime = new TimeSpan(9, 0, 0),  CloseTime = new TimeSpan(17, 0, 0) },
        new() { HourId = 6, DayOfWeek = "Saturday",  IsClosed = false, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(18, 0, 0) },
        new() { HourId = 7, DayOfWeek = "Sunday",    IsClosed = true,  SpecialNote = "Closed" },
    ];

    private static readonly List<FAQ> Faqs =
    [
        new() { FaqId = 1, Question = "What are the opening hours?", Answer = "See the schedule above.", Category = "General", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
        new() { FaqId = 2, Question = "Where is the museum located?", Answer = "Check the Visit page for directions.", Category = "Visit", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
    ];

    public IReadOnlyList<OpeningHour> GetOpeningHours() => OpeningHours;

    public IReadOnlyList<FAQ> GetActiveFaqs() =>
        Faqs.Where(f => f.IsActive).OrderBy(f => f.DisplayOrder).ToList();
}