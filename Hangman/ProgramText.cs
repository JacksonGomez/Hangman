using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
	partial class Program
	{
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

		const string LinebreakString = "█████████████████████████████████████████████████████████████████████████████████";

		static void ConsoleLinebreak()
		{
			Console.WriteLine(LinebreakString);
		}
	}
}
