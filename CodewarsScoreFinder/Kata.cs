
namespace CodewarsScoreFinder
{
    public class Kata
    {
        public Kata(string id, string name, string nameWithoutSpaces, string url)
        {
            initialize(id, name, nameWithoutSpaces, url);
        }
        public Kata(string id, string name, string nameWithoutSpaces)
        {
            initialize(id, name, nameWithoutSpaces, "");
        }
        private void initialize(string id, string name, string nameWithoutSpaces, string url)
        {
            Id = id;
            Name = name;
            NameWithoutSpaces = nameWithoutSpaces;
            Url = url;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameWithoutSpaces { get; set; }
        public string Url { get; set; }
    }
}
