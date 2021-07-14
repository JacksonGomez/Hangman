
using System;
using System.Collections.Generic;
using static System.Console;
using System.Threading;
using System.Text;
using System.IO;

namespace Hangman
{
	static partial class Program
	{
		static string LettersReplace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		static string LettersGuessed = "";
		static string WordGuess = "";
		static string LastGuess = "";
		static int GuessesRemaining = (0);
		static int StreakMaximum = LoadStreakMaximum();
		static int StreakCurrent = 0;
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
			Sound.SoundExecutePlayMusic();
			while (GameContinue)
            {
				GameStart();
            }
			Console.WriteLine("thanks for playing, your highest streak was {0}.", StreakMaximum);
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
			LastGuess = "";
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
				if (LastGuess != "")
				{
					if (GetStringIsInWord(LastGuess))
					{
						Console.ForegroundColor= ConsoleColor.Green;
						WriteLine("Good guess! There was a {0}.", LastGuess);
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						WriteLine("There were no {0}'s.", LastGuess);
						Console.ForegroundColor = ConsoleColor.White;
					}
				}
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
			StreakCurrent++;
			if (StreakMaximum < StreakCurrent)
            {
				StreakMaximum = StreakCurrent;
            }
			SaveStreakMaximum();
			ShowStreak();
			GameContinue = GetConfirmation("Do you want to play again? [Y/N]");
		}

		static void GameLose()
        {
			ClearConsole();
			WriteLine("bad job dude, the word was {0}.", WordGuess);
			StreakCurrent = 0;
			ShowStreak();
			GameContinue = GetConfirmation("Do you want to play again? [Y/N]");
		}

		static string GameTakeInput()
        {
			string ReturnValue = "";
			while (ReturnValue == "")
            {
				WriteLine("Guess a letter. You have {0} guesses remaining.", GuessesRemaining, LettersGuessed);
				if (LettersGuessed.Length > 0)
                {
					WriteLine("You have guessed the following letters so far: {0}", LettersGuessed);
                }
				string CheckInput = ReadLine().ToUpper();
				
				if (CheckInput.Length == 1)
                {
					string CharacterString = char.ToString(CheckInput[0]);
					if (LettersGuessed.Contains(CheckInput[0]))
                    {
						WriteLine("You have already guessed that letter.");
                    }
					else
                    {
						if (!GetStringIsInWord(CharacterString))
						{
							GuessesRemaining--;
						}
						LastGuess = CharacterString;
						ReturnValue = CharacterString;
					}
                }
				else
                {
					WriteLine("Invalid input.");
                }
            }

			ClearConsole();
			return ReturnValue;
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
			Console.ForegroundColor = ConsoleColor.White;
			Clear();
			ConsoleLinebreak();
			WriteLine(TitleString);
			ConsoleLinebreak();
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

		static bool GetStringIsInWord(string Input)
		{
			return (WordGuess.ToUpper().IndexOf(Input.ToUpper()) != -1);
		}

		static void ShowStreak()
		{
			WriteLine("current streak: {0}", StreakCurrent);
			WriteLine("maximum streak: {0}", StreakMaximum);
		}

		static void SaveStreakMaximum()
        {
			string FileDirectory = (Environment.GetEnvironmentVariable("APPDATA") + "\\GomezHangman\\");
			if (!Directory.Exists(FileDirectory))
            {
				Directory.CreateDirectory(FileDirectory);
            }
			using (StreamWriter OutputFile = new StreamWriter(Path.Combine(FileDirectory, "HIGHEST_STREAK.txt"), true))
			{
				OutputFile.WriteLine(StreakMaximum.ToString());
			}
		}

		static int LoadStreakMaximum()
        {
			string FilePath = (Environment.GetEnvironmentVariable("APPDATA") + "\\GomezHangman\\HIGHEST_STREAK.txt");
			int ReturnValue = 0;
			try
            {
				string InputString = File.ReadAllText(FilePath);
				ReturnValue = Int32.Parse(InputString);
			}
			catch
            {
				ReturnValue = 0;
            }
            return ReturnValue;
		}
	}
}