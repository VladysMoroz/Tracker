namespace Tracker.Entitites.ViewModels
{
    public class GoalForEditingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime DailyLimit { get; set; }
    }
}
