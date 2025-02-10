namespace Tracker.Entitites
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
