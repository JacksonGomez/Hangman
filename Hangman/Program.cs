
using System;
using System.Collections.Generic;
using static System.Console;
using System.Threading;
using System.Text;

namespace Hangman
{
	static class Program
	{
		static string LettersReplace = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		static string LettersGuessed = "";
		static string WordGuess = "";
		static int GuessesRemaining = (5);
		static bool GameContinue = true;
		static Random RandomObject = new Random();

		static readonly string[] AllWordsFood = Properties.Resources.food.Split("\n");
		static readonly string[] AllWordsOccupations = Properties.Resources.Occupations.Split("\n");
		static readonly string[] AllWordsMusic = Properties.Resources.music_genres.Split("\n");
		static readonly string[] AllWordsCelebrities = Properties.Resources.Celebrities.Split("\n");
		static readonly string[] AllWordsSciFiLoad = Properties.Resources.scifi.Split("\n");
		static string[] AllWordsSciFi;



		const string TitleString = @"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
   @@     @@                                                                    
   @@     @@                                                                    
   @@     @@    @@@@     @@@@@@      @@@@@  @@@@@@  @@@@     @@@@      @@@@@    
   @@@@@@@@@        @@   @@    @@  @@   @@  @@    @@    @@       @@    @@   @@  
   @@     @@    @@@@@@   @@    @@  @@   @@  @@    @@    @@   @@@@@@    @@   @@  
   @@     @@  @@    @@   @@    @@  @@   @@  @@    @@    @@  @@   @@    @@   @@  
   @@     @@  @@    @@   @@    @@  @@   @@  @@    @@    @@  @@   @@    @@   @@  
   @@     @@    @@@@ @@@ @@    @@    @@@@@  @@    @@    @@   @@@@  @@  @@   @@  
                                        @@                                      
                                   @@   @@                                      
                                     @@@@                                                                                                                                                                                
		";

		static void Main()
        {
			AllWordsSciFi = new string[AllWordsSciFiLoad.Length];
			for (int i = 0; i < AllWordsSciFiLoad.Length; i++)
			{
				AllWordsSciFi[i] = AllWordsSciFiLoad[i].Trim();
			}

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
			string Username = Environment.GetEnvironmentVariable("USERNAME");
			ClearConsole();

			Random randomCat = new Random();
			List<string> Category = new List<string> { /*"Food & Drink", "Occupations",*/ "Science Fiction", /*"Music", "Celebrities"*/ };
			int CatIndex = randomCat.Next(Category.Count);
			GamePickWord(Category, CatIndex);
			
			while (GuessesRemaining > 0)
			{
				ClearConsole();
				WriteLine("Category: {0}", Category[CatIndex]);
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
		
		static void GamePickWord(List<string> Category, int Index)
        {
			switch (Category[Index])
			{
				case "Food & Drink":
					WordGuess = AllWordsFood[RandomObject.Next(AllWordsFood.Length)];
					break;
				case "Occupations":
					WordGuess = AllWordsOccupations[RandomObject.Next(AllWordsOccupations.Length)];
					break;
				case "Science Fiction":
					WordGuess = AllWordsSciFi[RandomObject.Next(AllWordsSciFi.Length)];
					break;
				case "Music":
					WordGuess = AllWordsMusic[RandomObject.Next(AllWordsMusic.Length)];
					break;
				case "Celebrities":
					WordGuess = AllWordsCelebrities[RandomObject.Next(AllWordsCelebrities.Length)];
					break;
			}

			WriteLine(WordGuess);
		}
	}
}
