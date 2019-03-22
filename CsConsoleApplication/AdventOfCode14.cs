using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsConsoleApplication
{
    class AdventOfCode14
    {
        //private static RecipesLinkedList _recipes;

        public static void Run1(bool isTest = true)
        {
           var results = PrepareInput1(isTest);

           var recipesScoreBoard = RecipesScoreBoard.Instance;
           foreach (var result in results)
            {
                var tenRecipesScore = recipesScoreBoard.GetTenRecipesScore(result.RecipesQty);
                Console.WriteLine(String.Format("The scores of the ten recipes after {0} is {1} (must be {2})", result.RecipesQty, tenRecipesScore, result.Score));
                Console.ReadLine();
            }
        }

        public static void Run2(bool isTest = true)
        {
            var results = PrepareInput2(isTest);

            var recipesScoreBoard = RecipesScoreBoard.Instance;
            foreach (var result in results)
            {
                var recipesQty = recipesScoreBoard.GetFirstRecipesWithScore(result.Score);
                Console.WriteLine(String.Format("{0} recipes appeared before score {1} (must be {2})", recipesQty, result.Score, result.RecipesQty));
                Console.ReadLine();
            }
        }

        public static List<(int RecipesQty, string Score)> PrepareInput1(bool isTest)
        {
            var results = isTest ? ReadTestInput1() : ReadInput1();

            return results;
        }
        public static List<(int RecipesQty, string Score)> ReadTestInput1()
        {
            var input = new List<(int RecipesQty, string Score)>
            {
                (9, "5158916779"),
                (5, "0124515891"),
                (18, "9251071085"),
                (2018, "5941429882"),
            };
            return input;
        }
        public static List<(int RecipesQty, string Score)> ReadInput1()
        {
            var input = new List<(int RecipesQty, string Score)> { (440231, "") };
            return input;
        }

        public static List<(int RecipesQty, string Score)> PrepareInput2(bool isTest)
        {
            var results = isTest ? ReadTestInput2() : ReadInput2();

            return results;
        }
        public static List<(int RecipesQty, string Score)> ReadTestInput2()
        {
            var input = new List<(int RecipesQty, string Score)>
            {
                (9, "51589"),
                (5, "01245"),
                (18, "92510"),
                (2018, "59414"),
            };
            return input;
        }
        public static List<(int RecipesQty, string Score)> ReadInput2()
        {
            var input = new List<(int RecipesQty, string Score)>
            { 
                (0, "440231"),
            };
            return input;
        }
    }

    class RecipesScoreBoard
    {
        private static readonly Lazy<RecipesScoreBoard> _recipesScoreBoard = new Lazy<RecipesScoreBoard>(() => new RecipesScoreBoard());

        public static RecipesScoreBoard Instance { get { return _recipesScoreBoard.Value; } }

        private const int ScoreQty = 10;
        private int _firstElf;
        private int _secondElf;
        private int _count;
        private int[] _recipes;

        private RecipesScoreBoard()
        {
            int firstElement = 3;
            int secondElement = 7;

            _recipes = new int[2];

            _recipes[0] = firstElement;
            _recipes[1] = secondElement;
            _count = 2;

            _firstElf = 0;
            _secondElf = 1;
        }

        private void PrintState()
        {
            Console.WriteLine(string.Join(" ", _recipes.Take(_count).Select((r, i) => (i != _firstElf ? (i != _secondElf ? r.ToString() : "[" + r + "]") : "(" + r + ")"))));
        }

        private int AddRecipe()
        {
            if (_count > _recipes.Length - 2)
            {
                var temp_recipes = new int[_recipes.Length * 2];
                Array.Copy(_recipes, temp_recipes, _count);
                _recipes = temp_recipes;
            }

            int newRecipe = _recipes[_firstElf] + _recipes[_secondElf];

            if (newRecipe > 9)
            {
                _recipes[_count] = 1;
                _count++;
            }
            _recipes[_count] = newRecipe % 10;
            _count++;

            _firstElf = (_firstElf + _recipes[_firstElf] + 1) % _count;
            _secondElf = (_secondElf + _recipes[_secondElf] + 1) % _count;

            return _count;
        }

        public string GetTenRecipesScore(int afterRecipesQty)
        {
            if (_recipes.Length < afterRecipesQty + ScoreQty + 1)
            {
                var temp_recipes = new int[afterRecipesQty + ScoreQty + 1];
                Array.Copy(_recipes, temp_recipes, _count);
                _recipes = temp_recipes;
            }

            int recipesQty;
            while ((recipesQty = AddRecipe()) < afterRecipesQty + ScoreQty)
            {
                if (afterRecipesQty < 100) PrintState();
            }

            return string.Join("", _recipes.Skip(afterRecipesQty).Take(ScoreQty));
        }

        public int GetFirstRecipesWithScore(string score)
        {
            var scoreAsArray = score.ToArray().Select(c => int.Parse(c.ToString())).ToArray();

            if (_count > scoreAsArray.Length)
            {
                for (int i = 0; i < _count - scoreAsArray.Length; i++)
                {
                    bool good = true;
                    for (int j = 0; j < scoreAsArray.Length; j++)
                    {
                        if (_recipes[i + j] != scoreAsArray[j])
                        {
                            good = false;
                            break;
                        }
                    }
                    if (good)
                        return i;
                }
            }

            int count = _count;
            while (true)
            {
                int newCount = AddRecipe();

                if (newCount > scoreAsArray.Length)
                {
                    for (int offset = newCount - count; offset > 0; offset--)
                    {
                        bool good = true;
                        for (int i = 0; i < scoreAsArray.Length; i++)
                        {
                            if (_recipes[_count - (scoreAsArray.Length + offset - 1) + i] != scoreAsArray[i])
                            {
                                good = false;
                                break;
                            }
                        }
                        if (good)
                            return (_count - (scoreAsArray.Length + offset - 1));
                    }
                }

                count = newCount;
            }
        }
    }

}
