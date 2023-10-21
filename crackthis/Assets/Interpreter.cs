using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Numerics;

public class Interpreter : MonoBehaviour
{
    public GAMEMANAGER gamemanager;

    List<string> response = new List<string>();

    Dictionary<string, string> colors = new Dictionary<string, string>()
    {
        {"black", "#021b21"},
        {"gray", "#808080" },
        {"yellow", "#FFFF00" },
        {"green",  "#1BFF00"},
        {"red", "#FF0000"}
    };

    public List<string> Interpret(string userInput)
    {
        response.Clear();

        userInput = userInput.Trim();

        string[] args = userInput.Split();


        if (args[0] == "help")
        {
            ListEntry("help", "returns a list of commands");

            ListEntry("credits", "credit of terminal");

            ListEntry("methods", "return a list of methods");

            ListEntry("store \"-text\"", "store text for encrypt");

            ListEntry("clear", "clear stored text");

            ListEntry("last", "shows last stored text");

            ListEntry("lastbyte", "show last stored text as bytes");

            return response;
        }
        else if(args[0] == "methods")
        {

            ListEntry("atbash -e", "encrypt text with atbash");

            ListEntry("atbash -d", "decrypt text with atbash");

            ListEntry("caesar -e \"-int\"", "encrypt text with caesar");

            ListEntry("caesar -d \"-int\"", "decrypt text with caesar");

            ListEntry("vigenere -e \"-text\"", "encrypt text with vigenere");

            ListEntry("vigenere -d \"-text\"", "decrypt text with vigenere");

            ListEntry("rsi -c \"-prime number int\" \"-prime number int\" \"-int\"", "create rsi keys");

            ListEntry("rsi -e", "encrypt text with rsi public key");

            ListEntry("rsi -d", "decrypt text with rsi private key");

            return response;
        }
        else if (args[0] == "store")
        {
            string text = "";

            for (int i = 1; i < args.Length; i++)
            {
                text += args[i] + " ";
            }

            text = text.Trim();

            gamemanager.textThatWannaCrypted = text;
            gamemanager.textChars = text.ToCharArray();

            response.Add($"stored -> \"{text}\"");

            return response;

        }
        else if (args[0] == "last")
        {
            if (gamemanager.textThatWannaCrypted != "")
            {
                response.Add($"last text was {ColorString(gamemanager.textThatWannaCrypted, "green")}.");
            }
            else
            {
                response.Add("Could not found last text.");
            }

            return response;
        }
        else if (args[0] == "credits")
        {
            LoadTitle("ascii.txt", "red", 2);
            return response;
        }else if(args[0] == "lastbyte")
        {
            if (gamemanager.textThatWannaCrypted != "")
            {
                response.Add(gamemanager.stringToByteText(gamemanager.textThatWannaCrypted, 1));
            }
            else
            {
                response.Add("Could not found last text.");
            }
            return response;

        }else if(args[0] == "clear")
        {
            gamemanager.textThatWannaCrypted = "";
            response.Add("Stored text cleared.");
            return response;
        }
        if(gamemanager.textThatWannaCrypted != "")
        {
            if(args[0] == "yagiz")
            {
                string text = gamemanager.textThatWannaCrypted;
                gamemanager.textThatWannaCrypted = gamemanager.yagizmethod(text);
                response.Add($"{text} {ColorString("encrypted with yagiz", "green")} -> {gamemanager.textThatWannaCrypted}");
                return response;
            }
            else if (args[0] == "atbash" && args.Length == 2)
            {
                if (args[1] == "-e")
                {
                    string text = gamemanager.textThatWannaCrypted;
                    gamemanager.AtbashEncryptVoid();
                    response.Add($"{text} {ColorString("encrypted with atbash", "green")} -> {gamemanager.textThatWannaCrypted}");
                }
                else if (args[1] == "-d")
                {
                    string text = gamemanager.textThatWannaCrypted;
                    gamemanager.AtbashEncryptVoid();
                    response.Add($"{text} {ColorString("decrypted with atbash", "green")} -> {gamemanager.textThatWannaCrypted}");
                }
                else
                {
                    response.Add("Command not recognized. Type help for a list of commands.");
                }
                return response;
            }
            else if (args[0] == "caesar")
            {
                if (args.Length > 2)
                {
                    if (int.TryParse(args[2], out int a) && args.Length == 3)
                    {
                        if (args[1] == "-e")
                        {
                            string text = gamemanager.textThatWannaCrypted;
                            gamemanager.CaesarEncryptVoid(a);
                            response.Add($"{text} {ColorString("encrypted with caesar", "green")} -> {gamemanager.textThatWannaCrypted}");
                        }
                        else if (args[1] == "-d")
                        {
                            string text = gamemanager.textThatWannaCrypted;
                            gamemanager.CaesarDecryptVoid(a);
                            response.Add($"{text} {ColorString("decrypted with caesar", "green")} -> {gamemanager.textThatWannaCrypted}");
                        }
                    }
                }
                return response;
            }
            else if (args[0] == "vigenere")
            {
                if (args.Length > 2)
                {
                    if (args[1] == "-e")
                    {
                        if (args[2].All(char.IsLetter))
                        {
                            string text = gamemanager.textThatWannaCrypted;
                            gamemanager.VigenereEncryptVoid(args[2]);
                            response.Add($"{text} {ColorString("encrypted with vigenere", "green")} -> {gamemanager.textThatWannaCrypted}");
                        }
                    }
                    else if (args[1] == "-d")
                    {
                        if (args[2].All(char.IsLetter))
                        {
                            string text = gamemanager.textThatWannaCrypted;
                            gamemanager.VigenereDecryptVoid(args[2]);
                            response.Add($"{text} {ColorString("encrypted with vigenere", "green")} -> {gamemanager.textThatWannaCrypted}");
                        }
                    }
                }
                return response;
            }
            else if (args[0] == "rsi")
            {
                if (args.Length == 5)
                {
                    if (BigInteger.TryParse(args[2], out BigInteger a) && BigInteger.TryParse(args[3], out BigInteger b) && BigInteger.TryParse(args[4], out BigInteger e))
                    {
                        gamemanager.CreatePublicKey(a, b, e);
                        response.Add("Public key created.");
                    }

                }
                else if (args.Length == 2)
                {
                    if (args[1] == "-e")
                    {
                        string text = gamemanager.textThatWannaCrypted;
                        string cipherText = gamemanager.EncryptTextWithPublicKey(gamemanager.pKey.e, gamemanager.pKey.N, text);
                        gamemanager.textThatWannaCrypted = cipherText;
                        response.Add($"{text} {ColorString("encrypted with rsi using public key", "green")} -> {gamemanager.textThatWannaCrypted}");
                    }
                    else if (args[1] == "-d")
                    {
                        string text = gamemanager.textThatWannaCrypted;
                        string cipherText = gamemanager.DecryptTextWithPrivateKey(gamemanager.pKey.e, gamemanager.pKey.N, text, gamemanager.pKey.binaryA, gamemanager.pKey.binaryB);
                        gamemanager.textThatWannaCrypted = cipherText;
                        response.Add($"{text} {ColorString("encrypted with rsi using public key", "green")} -> {gamemanager.textThatWannaCrypted}");
                    }
                }
                return response;
            }
        }
        else
        {
            response.Add("Command not recognized. Type help for a list of commands.");
        }
        return response;
    }

    public string ColorString(string s, string color)
    {
        string leftTag = "<color=" + color + ">";
        string rightTag = "</color>";

        return leftTag + s + rightTag;
    }

    void ListEntry(string a, string b)
    {
        response.Add(ColorString(a, colors["red"]) + ": " + ColorString(b, colors["yellow"]));
    }

    void LoadTitle(string path, string color, int spacing)
    {
        StreamReader file = new StreamReader(Path.Combine(Application.streamingAssetsPath, path));

        for(int i = 0; i < spacing; i++)
        {
            response.Add("");
        }

        while(!file.EndOfStream)
        {
            response.Add(ColorString(file.ReadLine(), colors[color]));
        }

        for (int i = 0; i < spacing; i++)
        {
            response.Add("");
        }

        file.Close();
    }
}
