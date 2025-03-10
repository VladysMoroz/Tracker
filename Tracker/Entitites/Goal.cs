using System.Reflection.Metadata.Ecma335;

namespace Tracker.Entitites
{
    public class Goal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DeadLine { get; set; }
        public TimeSpan DailyLimit { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
