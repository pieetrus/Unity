using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hacker : MonoBehaviour
{
    // Game configuration data
    const string menuHint = "Możesz wpisać menu kiedy tylko chcesz.";
    string[] level1Passwords = { "książki", "półka", "biurko", "hasło", "czcionka", "pożyczyć" };
    string[] level2Passwords = { "więzień", "kajdanki", "pałka", "mundur", "aresztować" };
    string[] level3Passwords = { "teleskop", "środowisko", "uprzywilejowany", "rakieta", "astronauta" };

    // Game state
    int level;
    enum Screen { MainMenu, Password, Win };
    Screen currentScreen;
    string password;

    // Use this for initialization
    void Start()
    {
        ShowMainMenu();
    }

    void ShowMainMenu()
    {
        currentScreen = Screen.MainMenu;
        Terminal.ClearScreen();
        Terminal.WriteLine("Gdzie chciałbyś się włamać?");
        Terminal.WriteLine("Wybierz 1 aby włamać się do biblioteki");
        Terminal.WriteLine("Wybierz 2 aby włamać się na posterunek policji");
        Terminal.WriteLine("Wybierz 3 aby włamać się do NASA!");
        Terminal.WriteLine("Wpisz swój wybór:");
    }

    void OnUserInput(string input)
    {
        if (input == "menu") // we can always go direct to main menu
        {
            ShowMainMenu();
        }
        else if (input == "quit" || input == "close" || input == "exit"
                 || input == "wyjdź" || input == "koniec")
        {
            Application.Quit();
        }
        else if (currentScreen == Screen.MainMenu)
        {
            RunMainMenu(input);
        }
        else if (currentScreen == Screen.Password)
        {
            CheckPassword(input);
        }
    }

    void RunMainMenu(string input)
    {
        bool isValidLevelNumber = (input == "1" || input == "2" || input == "3");
        if (isValidLevelNumber)
        {
            level = int.Parse(input);
            AskForPassword();
        }
        else if (input == "007") // easter egg
        {
            Terminal.WriteLine("Wybierz poziom Mr Bond!");
        }
        else
        {
            Terminal.WriteLine("Wybierz właściwy numer poziomu");
            Terminal.WriteLine(menuHint);
        }
    }

    void AskForPassword()
    {
        currentScreen = Screen.Password;
        Terminal.ClearScreen();
        SetRandomPassword();
        Terminal.WriteLine("Wpisz hasło, wskazówka: " + password.Anagram());
        Terminal.WriteLine(menuHint);
    }

    void SetRandomPassword()
    {
        switch (level)
        {
            case 1:
                password = level1Passwords[Random.Range(0, level1Passwords.Length)];
                break;
            case 2:
                password = level2Passwords[Random.Range(0, level2Passwords.Length)];
                break;
            case 3:
                password = level3Passwords[Random.Range(0, level3Passwords.Length)];
                break;
            default:
                Debug.LogError("Zły numer poziomu");
                break;
        }
    }

    void CheckPassword(string input)
    {
        if (input == password)
        {
            DisplayWinScreen();
        }
        else
        {
            AskForPassword();
        }
    }

    void DisplayWinScreen()
    {
        currentScreen = Screen.Win;
        Terminal.ClearScreen();
        ShowLevelReward();
        Terminal.WriteLine(menuHint);
    }

    void ShowLevelReward()
    {
        switch (level)
        {
            case 1:
                Terminal.WriteLine("Masz książkę...");
                Terminal.WriteLine(@"
    _______
   /      //
  /      //
 /_____ //
(______(/           
"
                );
                break;
            case 2:
                Terminal.WriteLine("Masz klucz do więzienia!");
                Terminal.WriteLine("Zagraj jeszcze raz aby zwiększyć poziom wyzwania.");
                Terminal.WriteLine(@"
 __
/0 \_______
\__/-=' = '         
"
                );
                break;
            case 3:
                Terminal.WriteLine(@"
 _ __   __ _ ___  __ _
| '_ \ / _` / __|/ _` |
| | | | (_| \__ \ (_| |
|_| |_|\__,_|___)\__,_|
"
                );
                Terminal.WriteLine("Witaj w systemie NASA!");
                break;
            default:
                Debug.LogError("Niepoprawny poziom osiągnięty");
                break;
        }
    }
}
