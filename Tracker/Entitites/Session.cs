namespace Tracker.Entitites
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartSession { get; set; }
        public DateTime? EndSession { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
