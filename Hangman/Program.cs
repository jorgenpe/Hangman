using System;
using System.Text;
using System.IO;

namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {

            bool choiceFileArray = true;
            bool choice = false;
            
            // Handling of main menu choices
            while (!choice)
            {
                Console.Clear();
                int menuChoice = MainMenu();
                switch(menuChoice){

                    case 1:
                        {
                            
                            Hangman(choiceFileArray);
                            break;
                        }

                    case 2:
                        {
                            choiceFileArray = false;
                            Hangman(choiceFileArray);
                            break;
                        }
                    case 3:
                        {
                            choice = true;
                            break;
                        }
                }
            }
            //Main menu
            static int MainMenu()
            {
                Console.WriteLine("Welcome to Hangman! Please select on of the following chooses: \n");
                Console.WriteLine(" 1.  Guess random word from array of string game");
                Console.WriteLine(" 2.  Guess word from random text file word game");
                Console.WriteLine(" 3.  Exit");
                return TestIfInt();
            }
            // Test input if it is an integer
            static int TestIfInt()
            {
                bool notInt = true;
                int result = 0;
                do
                {
                    try
                    {
                        result = int.Parse(Console.ReadLine());
                        notInt = false;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine(" Your input was not a integer. Please input a integer, example 1 or 2.");
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("The number you inputed is to big." );
                    }
                } while (notInt);
                return result;
            }
            // Decides if external text file should be used
            static void Hangman(bool choice)
            {
                if (choice)
                {
                    InputWords();
                }
                else
                {
                    TextFile();
                }
            }
            // Handle choices from second menu
            static void InputWords()
            {
                
                bool choice = false;

                
                while(!choice)
                {
                    Console.Clear();
                    int inputMenu = InputMenu();
                    switch (inputMenu)
                    {
                        case 1:
                            {
                                string[] inputWords = InputWord();
                                Hanging(inputWords);
                                break;
                            }

                        case 2:
                            {
                                string[] hardCodeWord = HardCodeWord();
                                Hanging(hardCodeWord);
                                break;
                            }

                        case 3:
                            {
                                choice = true;
                                break;
                            }
                    }
                }
            }
            // Second menu
            static int InputMenu()
            {
                Console.WriteLine("Please select what you want to do: \n");
                Console.WriteLine(" 1.  Do you want to input words to guess from. ");
                Console.WriteLine(" 2.  Do you want to use words that are already in the game.");
                Console.WriteLine(" 3.  Exit.");
                return TestIfInt();
            }

            // Method that reads in a text file.
            static void TextFile()
            {
                string[] word;
                string text;
                char[] charText;
                int index = 0;
                Console.WriteLine("Please give the path to your textfile." + @"C:\Users\Public\WriteLines2.txt ");
                string path = Console.ReadLine();
                try
                {

                    text = File.ReadAllText(@path);
                    charText = new char[text.Length];
                    charText = text.ToCharArray();
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < charText.Length; i++)
                    {
                        if (charText[i] == ',')
                        {
                            index++;
                        }
                    }

                    int count = 0;
                    word = new string[index + 1];
                    for (int i = 0; i < charText.Length; i++)
                    {
                        if (charText[i] == ',')
                        {
                            word[count++] = stringBuilder.ToString();
                            stringBuilder = new StringBuilder();
                        }
                        else if (i == charText.Length - 1)
                        {
                            word[count++] = stringBuilder.ToString();
                        }
                        else
                        {
                            stringBuilder.Append(charText[i]);
                        }
                    }
                   
                    Hanging(word);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Some thing has gone wrong.");
                }
            }
            // Choose how many words you want to put in
            static string[] InputWord()
            {
                Console.WriteLine("How many words do you want to input");
                int length = TestIfInt();
                string[] inputWords = new string[length];
                for (int i = 0;i < length; ++i)
                {
                    Console.WriteLine("Write in your word number " + (i + 1) + ": ");
                    inputWords[i] = Console.ReadLine();
                }
                return inputWords;


            }
            // Hard coded words
            static string[] HardCodeWord(){

                string [] inputWords = new string[5];
                inputWords[0] = "Rar";
                inputWords[1] = "God";
                inputWords[2] = "Rosor";
                inputWords[3] = "Träd";
                inputWords[4] = "Växter";

                return inputWords;

            }
            // Main game method
            static void Hanging(string[] inputWords)
            {
                
                string selectedWord = RandomWord(inputWords);
                StringBuilder charInput = new StringBuilder();
                string userInput;
                char[] result = StartValue(selectedWord);

                
                for (int i = 0;i < 10; i++)
                {
                    Console.WriteLine("\n");
                    userInput = UserInput(i);


                    if (selectedWord.ToLower().Equals(userInput.ToLower()))
                    {
                        Console.WriteLine(" You escape the hangman and you keep your life! Congatulation! You have won the game!");
                        Console.ReadKey();
                        break;
                    }else if (userInput.Length == 1)
                    {
              
                        if (!CompareChar(charInput.ToString(), userInput) && !CompareChar(selectedWord, userInput))
                        {
                            charInput.Append(" " + userInput);

                            PrintResult(ToString(result), charInput.ToString());

                        }
                        else if(CompareChar(selectedWord, userInput)){
                            if (CompareChar(ToString(result), userInput) && i > 0){
                                i--;
                            }
                            else
                            {
                                result = Result(selectedWord, result, userInput);
                                PrintResult(ToString(result), charInput.ToString());
                            }
                        }
                        else if(i > 0 ) {

                            PrintResult(ToString(result), charInput.ToString());

                            i--;
                        }
                    }else if (!(selectedWord.ToLower().Equals(userInput.ToLower()))){

                        Console.WriteLine(" The word you guessed is wrong.");
                    }


                    if (ToString(result).ToLower().Equals(selectedWord.ToLower()))
                    {
                        Console.WriteLine(" You escape the hangman and you keep your life! Congatulation! You have won the game!");
                        Console.ReadKey();  
                        break;
                    }

                    if(i == 9)
                    {
                        Console.WriteLine(" You hang by your neck until your dead! Game over!");
                        Console.ReadKey();
                        break;
                    }                    
                }
            }
            // Print out result
            static void PrintResult(string result, string guessedChar)
            {

                Console.WriteLine(" Letters guessed:" + guessedChar);
                Console.WriteLine(" Correct letters: " + result);

            }
            // Char arry to string
            static string ToString(char[] temp)
            {
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < temp.Length; i++)
                {
                    sb.Append(temp[i]);
                }
                return sb.ToString();
            }
            // Save correct result in a array
            static char[] Result(string selected, char[] result, string userInput)
            {
                
                for(int i = 0; i < selected.Length; i++)
                {
                    if (char.ToLower(userInput[0]) == char.ToLower(selected[i]))
                    {
                        result[i] = userInput[0]; 
                    }
                }
                return result;
            }
            // Initiate array with start value
            static char[] StartValue(string selectedWord)
            {
                char[] temp = new char[selectedWord.Length];
                for(int i = 0; i < selectedWord.Length; i++)
                {
                    temp[i] = ('_');

                }
                return temp;
            }
            // Compare a string elements with the first element in string two
            static bool CompareChar(string input1, string input2)
            {
                bool result = false;

                input1 = input1.ToLower();
                input2 = input2.ToLower();  

                for(int i = 0; i < input1.Length; i++)
                {
                    if (input1[i] == input2[0]) {
                        result = true; break; }   
                }

                return result;
            }   
            //Game strings depending on were in the game you are
            static string UserInput(int index)
            {
                if (index == 0)
                {
                    Console.WriteLine(" Welcome to Hangman ");
                    Console.WriteLine(" ------------------\n");

                }
               
                Console.WriteLine(" This is guess number " + (index + 1));
                Console.WriteLine(" Select a character you think is in the word we seek or the word you think it is:  ");
             return Console.ReadLine();
            }
            // Random generated strings from a array of strings
            static String RandomWord(string[] inputWords)
            { 
                Random random = new Random();                
                return inputWords[random.Next(inputWords.Length)];
            }
        }
    }
}
