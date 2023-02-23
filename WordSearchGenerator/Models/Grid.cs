namespace WordSearchGenerator.Models
{
    public class Grid
    {
        public int GridSize { get; set; }        
        public List<string> WordList { get; set; } = default!;
        public List<string> WordFit { get; set; } = default!;
        public List<string> WordNoFit { get; set; } = default!;
        public List<List<string>> MasterCoordinates { get; set; }

    }
}
