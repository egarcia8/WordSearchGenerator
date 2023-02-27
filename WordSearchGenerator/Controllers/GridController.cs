using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WordSearchGenerator.Models;

namespace WordSearchGenerator.Controllers
{
    public class GridController : Controller
    {
        /// <summary>
        /// Data received from home view saved as tempGrid
        /// </summary>
        /// <returns>"tempGrid"</returns>
        public IActionResult Index()
        {
            //use tempdata to store data temporarily and set it to temp
            var temp = TempData["UserInput"] as string;
            //when page is refreshed redirect to home index
            if (temp == null) return RedirectToAction("Index", "Home");

            //parse single JSON value into UserInput type
            var userInput = JsonSerializer.Deserialize<UserInput>(temp);            

            //Set both userInput properties to tempGrid
            var tempGrid = CreateGrid(userInput.Gridsize, userInput.WordList);

            return View(tempGrid);
        }

        /// <summary>
        /// Rebuild grid and word lists
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        [HttpPost("GetPartial")]
        //[FromBody] attribute used to specify that the value should be read from the body of the request
        public ActionResult GenerateGrid([FromBody] UserInput userInput)
        {
            var tempGrid = CreateGrid(userInput.Gridsize, userInput.WordList);
            return PartialView("_PartialGridView", tempGrid);
        }

        /// <summary>
        /// Get sizeGrid and wordList to rebuild puzzle
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        [HttpPost("api/grid")]
        //[FromBody] attribute used to specify that the value should be read from the body of the request
        public ActionResult GenerateGrid([FromBody]UserInput userInput)
        {
            var tempGrid = CreateGrid(userInput.Gridsize, userInput.WordList);
            return Ok(tempGrid);
        }

        /// <summary>
        /// Uses object returned to create grid and word lists
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        private Grid CreateGrid(int gridSize, List<string> wordList) {
           
            //Setup list for items that fit and did not fit
            var noFitList = new List<string>();
            var insertedList = new List<string>();

            var uppercaseWords = new List<string>();
            //Upper case all words
            foreach (var word in wordList)
            {
                uppercaseWords.Add(word.ToUpper());
            }


            var rand = new Random();
            //Setup catalog of Master Coordinates
            var catalogMasterCoordinates = new List<int[]>();
            //Setup Master Coordinates
            //Initiate all coordinates to "_"
            //And catalog coordinates
            var masterCoordinates = new string[gridSize, gridSize];
            for (int x = 0; x < masterCoordinates.GetLength(0); x++)
            {
                for (int y = 0; y < masterCoordinates.GetLength(1); y++)
                {
                    masterCoordinates[x, y] = "_";
                    catalogMasterCoordinates.Add(new[] { x, y });
                }
            }


            //Go through each word 
            foreach (var word in uppercaseWords)
            {
                //Shuffle catalog
                var shuffledListOfCoordinates = catalogMasterCoordinates.OrderBy(_ => rand.Next()).ToList();
                var wordInserted = false;
                // Go through each shuffled coordinate
                for (int i = 0; i < shuffledListOfCoordinates.Count; i++)
                {
                    if (wordInserted) break;
                    var coordinate = shuffledListOfCoordinates[i];

                    //Create list of directions and then shuffle them
                    var directions = new List<int[]> { Direction.Horizontal, Direction.Vertical, Direction.Diagonal };
                    var shuffledListOfDirections = directions.OrderBy(_ => rand.Next()).ToList();

                    //Go through each direction and check if a word will fit
                    //If not go to next coordinate
                    foreach (var direction in shuffledListOfDirections)
                    {
                        var willFit = CheckWillFit(word, coordinate, masterCoordinates, direction, gridSize);
                        if (willFit)
                        {
                            InsertWord(word, coordinate, masterCoordinates, direction);
                            wordInserted = true;
                            insertedList.Add(word);
                            break;
                        }
                    }

                    //If all of the coordinates have been looped through the word does not fit
                    if (shuffledListOfCoordinates.Count-1 == i)
                    {
                        noFitList.Add(word);
                    }
                }
            }

            //Replace all "_" with random characters to complete the puzzle
            for (int x = 0; x < masterCoordinates.GetLength(0); x++)
            {
                for (int y = 0; y < masterCoordinates.GetLength(1); y++)
                {
                    var value = masterCoordinates[x, y];
                    if (value == "_")
                    {
                        var randomLetter = ((char)rand.Next(65, 90)).ToString();
                        masterCoordinates[x, y] = randomLetter;
                    }
                }
            }

            var tempList = new List<List<string>>();
            for (int x = 0; x < masterCoordinates.GetLength(0); x++)
            {
                var yList = new List<string>();
                for (int y = 0; y < masterCoordinates.GetLength(1); y++)
                {
                    yList.Add(masterCoordinates[x, y]);
                }
                tempList.Add(yList);
            }


            //Return an object with all requirements
            return new Grid()
            {
               GridSize = gridSize,
                WordList = wordList,
                WordFit = insertedList,
                WordNoFit = noFitList,
                MasterCoordinates = tempList

            };
        }

        //Method to check if a word will fit
        static bool CheckWillFit(string word, int[] coordinate, string[,] masterCoordinates, int[] direction, int gridSize)
        {
            //Keep track of the current position
            var row = coordinate[0];
            var col = coordinate[1];

            //Checks to ensure starting coordinate will not go out of bounds
            foreach (char ch in word)
            {
                if (row >= gridSize || col >= gridSize)
                {
                    return false;
                }
                //Setting current coordinate equal to the Master Coordinate grid to check position
                var currentCoordinateValue = masterCoordinates[row, col];
                if (currentCoordinateValue != "_" && currentCoordinateValue != Char.ToString(ch))
                {
                    return false;
                }
                //Incrementing by direction(x,y)
                row = row + direction[0];//X
                col = col + direction[1];//Y
            }

            return true;
        }

        //Method to insert word into grid
        static void InsertWord(string word, int[] coordinate, string[,] masterCoordinates, int[] direction)
        {
            //Keep track of the current position
            var row = coordinate[0];
            var col = coordinate[1];
            
            //Insert each ch by starting with current position
            //Continue inserting at next position
            foreach (char ch in word)
            {
                masterCoordinates[row, col] = Char.ToString(ch);

                row = row + direction[0];//X
                col = col + direction[1];//Y
            }
        }
    }
    
    //All directions and how the position moves according to (x, y) value
    public static class Direction 
    { 
        public static int[] Horizontal = { 1, 0 };
        public static int[] Vertical = { 0, 1 };
        public static int[] Diagonal = { 1, 1 };
    }

}
