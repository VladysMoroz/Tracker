namespace Tracker.Entitites
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Session>? Sessions { get; set; }
        public List<Goal>? Goals { get; set; }
        public int UserId { get; set; }
        //public List<User> User { get; set; }
    }
}
