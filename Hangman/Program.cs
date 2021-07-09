
using System;
using System.Collections.Generic;
using static System.Console;
using System.Threading;
using System.Text;

namespace Hangman
{
	static partial class Program
	{
		static string LettersReplace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		static string LettersGuessed = "";
		static string WordGuess = "";
		static int GuessesRemaining = (0);
		static bool GameContinue = true;
		static Random RandomObject = new Random();

		static int CategoryIndex;
		static List<string> CategoryNames = new List<string> { "Food & Drink", "Occupations", "Music", "Celebrities", "SciFi" };
		static readonly string[][] CategoryMaster =
		{
			LoadCategoryWords(Properties.Resources.category_food),
			LoadCategoryWords(Properties.Resources.category_occupations),
			LoadCategoryWords(Properties.Resources.category_music),
			LoadCategoryWords(Properties.Resources.category_celebrities),
			LoadCategoryWords(Properties.Resources.category_scifi),
		};

		
		
		static void Main()
        {
			while (GameContinue)
            {
				GameStart();
            }
        }

		static bool CheckWin()
        {
			foreach (char Character in WordGuess)
            {
				char CharacterCheck = char.ToUpper(Character);
				if (!LettersGuessed.Contains(CharacterCheck))
                {
					if (LettersReplace.Contains(CharacterCheck))
					{
						return false;
					}
                }
            }
			return true;
        }

		static void GameStart()
        {
			LettersGuessed = "";
			WordGuess = "";
			GuessesRemaining = (5);
			GetCategoryIndex();
			GetRandomWord();
			string Username = Environment.GetEnvironmentVariable("USERNAME");
			ClearConsole();
			
			while (GuessesRemaining > 0)
			{
				ClearConsole();
				WriteLine("Category: {0}", GetCategoryName());
				WriteLine(WordReturnObscured());
				string InputLetter = GameTakeInput();

				LettersGuessed += InputLetter;

				if (CheckWin())
				{
					GameWin();
					break;
				}
			}

			if (!CheckWin())
			{
				GameLose();
			}
		}

		static void GameWin()
        {
			ClearConsole();
			WriteLine(WordReturnObscured());
			WriteLine("good job dude.");
			GameContinue = GetConfirmation("Do you want to play again? [Y/N]");
		}

		static void GameLose()
        {
			ClearConsole();
			WriteLine("bad job dude. the word was {0}.", WordGuess);
			GameContinue = GetConfirmation("Do you want to play again? [Y/N]");
        }

		static string GameTakeInput()
        {
			while (true)
            {
				WriteLine("Guess a letter. You have {0} guesses remaining.", GuessesRemaining, LettersGuessed);
				if (LettersGuessed.Length > 0)
                {
					WriteLine("You have guessed the following letters so far: {0}", LettersGuessed);
                }
				string CheckInput = ReadLine().ToUpper();
				if (CheckInput.Length == 1)
                {
					if (LettersGuessed.Contains(CheckInput[0]))
                    {
						WriteLine("You have already guessed that letter.");
                    }
					else
                    {
						if (!WordGuess.ToUpper().Contains(CheckInput[0]))
                        {
							GuessesRemaining--;
                        }
						return CheckInput;
					}
                }
				else
                {
					WriteLine("Invalid input.");
                }
            }
        }

		static string WordReturnObscured()
        {
			StringBuilder SB = new StringBuilder();
			string WordReturn = WordGuess;
			foreach (char Character in WordReturn)
			{
				char CharacterCheck = char.ToUpper(Character);
				char CharacterAppend = '?';
				if (!LettersReplace.Contains(CharacterCheck))
				{
					CharacterAppend = Character;
				}
				if (LettersGuessed.Contains(CharacterCheck))
				{
					CharacterAppend = Character;
				}
				SB.Append(CharacterAppend.ToString());
			}
			return SB.ToString();
		}

		static void ClearConsole()
        {
			Clear();
			WriteLine(TitleString);
			WriteLine("");
		}

		static bool GetConfirmation(string Message)
        {
			WriteLine(Message);
			while (true)
            {
				string Input = ReadLine().ToUpper();
				switch (Input)
                {
					case "YES":
					case "Y":
						return true;
					case "NO":
					case "N":
						WriteLine("Press Enter to close the window.");
						ReadLine();
						return false;
                }
            }
        }
		
		static void GetRandomWord()
        {
			string[] WordArray = CategoryMaster[CategoryIndex];
			int WordIndex = RandomObject.Next(WordArray.Length);
			WordGuess = WordArray[WordIndex];

			WriteLine(WordGuess);
		}

		static void GetCategoryIndex()
		{
			ClearConsole();
			WriteLine("Please select one of the below categories:");
			for (int i = 0; i < CategoryNames.Count; i++)
			{
				WriteLine("[{0}] - {1}", i + 1, CategoryNames[i]);
			}

			while (true)
			{
				string CategoryInput = ReadLine();
				try
				{
					CategoryIndex = Int32.Parse(CategoryInput) - 1;
					if ((CategoryIndex < 0) || (CategoryIndex >= CategoryNames.Count))
					{
						WriteLine("Invalid index. Try again.");
					}
					else
					{
						return;
					}
				}
				catch (Exception)
				{
					WriteLine("Invalid choice. Try again.");
				}
			}
		}

		static string[] LoadCategoryWords(string Input)
		{
			string[] LoadInput = Input.Split("\n");
			string[] LoadReturn = new string[LoadInput.Length];
			for (int i = 0; i < LoadInput.Length; i++)
			{
				LoadReturn[i] = LoadInput[i].Trim();
			}

			return LoadReturn;
		}

		static string GetCategoryName()
		{
			return CategoryNames[CategoryIndex];
		}
	}
}
