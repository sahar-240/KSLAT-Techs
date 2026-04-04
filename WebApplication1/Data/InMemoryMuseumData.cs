using WebApplication1.Models;

namespace WebApplication1.Data;

public sealed class InMemoryMuseumData : IMuseumData
{
    private static readonly List<OpeningHour> OpeningHours =
[
    new OpeningHour { Id = 1, DayOfWeek = "Monday",    IsClosed = false, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0) },
    new OpeningHour { Id = 2, DayOfWeek = "Tuesday",   IsClosed = false, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0) },
    new OpeningHour { Id = 3, DayOfWeek = "Wednesday", IsClosed = false, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0) },
    new OpeningHour { Id = 4, DayOfWeek = "Thursday",  IsClosed = false, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(20, 0, 0), Notes = "Extended hours" },
    new OpeningHour { Id = 5, DayOfWeek = "Friday",    IsClosed = false, OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0) },
    new OpeningHour { Id = 6, DayOfWeek = "Saturday",  IsClosed = false, OpeningTime = new TimeSpan(11, 0, 0), ClosingTime = new TimeSpan(18, 0, 0) },
    new OpeningHour { Id = 7, DayOfWeek = "Sunday",    IsClosed = false, OpeningTime = new TimeSpan(12, 0, 0), ClosingTime = new TimeSpan(17, 0, 0) },
];
    private static readonly List<FAQ> Faqs =
 [
     new()
    {
        FaqId = 1,
        Question = "DO I NEED TO BOOK IN ADVANCE?",
        Answer = "BOOKINGS ARE ONLY REQUIRED FOR TOURS AND LIMITED ACCESS EVENTS.\n\nTHIS IS TO PREVENT OVERCROWDING & PROVIDE GUIDES AND STAFF WITH A WORKABLE GROUP SIZE.\n\nVISITORS CAN ENJOY THE MAJORITY OF THE MUSEUM WITHOUT A BOOKING.",
        Category = "General",
        DisplayOrder = 1,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new()
    {
        FaqId = 2,
        Question = "CAN I BUY TICKETS AT ARRIVAL?",
        Answer = "YES, TICKETS ARE AVAILABLE FOR PURCHASE AT OUR RECEPTION DESK DURING OPENING HOURS.\n\nWE RECOMMEND BOOKING IN ADVANCE DURING PEAK TIMES TO AVOID QUEUES.",
        Category = "Visit",
        DisplayOrder = 2,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new()
    {
        FaqId = 3,
        Question = "ARE TICKETS REFUNDABLE?",
        Answer = "TICKETS ARE NON-REFUNDABLE ONCE PURCHASED. HOWEVER, YOU MAY EXCHANGE YOUR TICKET FOR ANOTHER DATE SUBJECT TO AVAILABILITY.\n\nPLEASE CONTACT US FOR MORE INFORMATION.",
        Category = "Visit",
        DisplayOrder = 3,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    },
    new()
    {
        FaqId = 4,
        Question = "IS THE MUSEUM SUITABLE FOR CHILDREN?",
        Answer = "THE MUSEUM IS SUITABLE FOR VISITORS OF ALL AGES.\n\nCHILDREN ARE ENCOURAGED TO EXPLORE THE EXHIBITS AND PARTICIPATE IN WORKSHOP EVENTS REGULARLY HOSTED BY THE MUSEUM.\n\nPARENTS AND CARERS ARE RESPONSIBLE FOR THEIR CHILD'S SAFETY AND MUST ACCOMPANY ANY CHILDREN UNDER THE AGE OF 13.",
        Category = "General",
        DisplayOrder = 4,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    }
 ];
    public IReadOnlyList<OpeningHour> GetOpeningHours() => OpeningHours;

    public IReadOnlyList<FAQ> GetActiveFaqs() =>
        Faqs.Where(f => f.IsActive).OrderBy(f => f.DisplayOrder).ToList();
}