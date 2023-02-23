namespace WordSearchGenerator.Models
{
    public class UserInput
    {
        public int Gridsize { get; set; }
        public List<string> WordList { get; set; } = default!;
    }
}
