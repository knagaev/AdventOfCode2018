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
            var recipesQtys = PrepareInput(isTest);

           var recipesScoreBoard = RecipesScoreBoard.Instance;
           foreach (var recipesQty in recipesQtys)
            {
                var tenRecipesScore = recipesScoreBoard.GetTenRecipesScore(recipesQty);
                Console.WriteLine(String.Format("The scores of the ten recipes after {0} is {1}", recipesQty, tenRecipesScore));
                Console.ReadLine();
            }

        }
        public static List<int> PrepareInput(bool isTest)
        {
            var results = isTest ? ReadTestInput() : ReadInput();

            return results;
        }
        public static List<int> ReadTestInput()
        {
            var input = new List<int> { 9, 5, 18, 2018 };
            return input;
        }
        public static List<int> ReadInput()
        {
            var input = new List<int> { 440231 };
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
    }

}
