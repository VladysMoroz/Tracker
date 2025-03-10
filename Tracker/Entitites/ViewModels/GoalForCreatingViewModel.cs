namespace Tracker.Entitites.ViewModels
{
    public class GoalForCreatingViewModel
    {
        public string Name { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime DailyLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
    }
}
