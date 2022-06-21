namespace Api.Tests
{
    public class Task
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Board board { get; set; }
        public string dateCreated { get; set; }
        public string dateModified { get; set; }
    }
}
