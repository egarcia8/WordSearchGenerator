namespace WordSearchGenerator.Models
{
    public class Grid
    {
        public int sizeGrid { get; set; }
        public int MyProperty { get; set; }
        public List<string> wordList { get; set; }
        public List<string> wordFit { get; set; }
        public List<string> wordNoFit { get; set; }

    }
}
