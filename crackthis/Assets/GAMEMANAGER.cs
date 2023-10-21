using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using System.Numerics;
using System;

public class GAMEMANAGER : MonoBehaviour
{
    int counter = 0;
    int timer = 0;

    public string alphabetLower = "abcdefghijklmnopqrstuvwxyz";
    public string alphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string key = "dobra";

    string text = "";

    public TextMeshProUGUI textUI;

    [TextArea(15, 20)]
    public string textThatWannaCrypted;
    public float letterAddTime;

    public char[] textChars;

    public class publicKey
    {
        public BigInteger binaryA;
        public BigInteger binaryB;
        public BigInteger e;
        public BigInteger N;
    }
    public publicKey pKey = new publicKey();

    public void CreatePublicKey(BigInteger a,BigInteger b,BigInteger e)
    {
        pKey.binaryA = a;
        pKey.binaryB = b;
        pKey.e = e;
        pKey.N = a * b;
    }

    public void AtbashEncryptVoid()
    {
        text = "";
        counter = 0;
        textThatWannaCrypted = AtbashEncrypt(textThatWannaCrypted);
        textChars = textThatWannaCrypted.ToCharArray();
    }

    public string AtbashEncrypt(string text)
    {
        string encryptedText = "";

        foreach (char character in text)
        {
            if (char.IsLetter(character))
            {
                if (char.IsLower(character))
                {
                    for (int i = 0; i < alphabetLower.Length; i++)
                    {

                        if (character == alphabetLower[i])
                        {
                            int reverse = alphabetLower.Length - i - 1;
                            encryptedText += alphabetLower[reverse];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < alphabetUpper.Length; i++)
                    {

                        if (character == alphabetUpper[i])
                        {
                            int reverse = alphabetUpper.Length - i - 1;
                            encryptedText += alphabetUpper[reverse];
                            break;
                        }
                    }
                }
            }
            else
            {
                encryptedText += character;
            }
        }
        return encryptedText;
    }

    public void CaesarEncryptVoid(int key)
    {
        text = "";
        counter = 0;
        textThatWannaCrypted = CaesarEncrypt(textThatWannaCrypted,key);
        textChars = textThatWannaCrypted.ToCharArray();
    }

    public void CaesarDecryptVoid(int key)
    {
        text = "";
        counter = 0;
        textThatWannaCrypted = CaesarDecrypt(textThatWannaCrypted, key);
        textChars = textThatWannaCrypted.ToCharArray();
    }

    public string CaesarEncrypt(string text, int shift)
    {
        string encryptedText = "";

        foreach (char character in text)
        {
            if (char.IsLetter(character))
            {
                if (char.IsLower(character))
                {
                    for (int i = 0; i < alphabetLower.Length; i++)
                    {
                        if (character == alphabetLower[i])
                        {
                            if (i + shift >= alphabetLower.Length)
                            {
                                int caesar = i + shift - alphabetLower.Length;
                                encryptedText += alphabetLower[caesar];
                                break;
                            }
                            else
                            {
                                int caesar = i + shift;
                                encryptedText += alphabetLower[caesar];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < alphabetUpper.Length; i++)
                    {
                        if (character == alphabetUpper[i])
                        {
                            if (i + shift >= alphabetUpper.Length)
                            {
                                int caesar = (i + shift) - alphabetUpper.Length;
                                encryptedText += alphabetUpper[caesar];
                                break;
                            }
                            else
                            {
                                int caesar = i + shift;
                                encryptedText += alphabetUpper[caesar];
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                encryptedText += character;
            }
        }
        return encryptedText;
    }

    public string CaesarDecrypt(string text, int shift)
    {
        string decryptedText = "";

        foreach (char character in text)
        {
            if (char.IsLetter(character))
            {
                if (char.IsLower(character))
                {
                    for (int i = 0; i < alphabetLower.Length; i++)
                    {
                        if (character == alphabetLower[i])
                        {
                            if(i - shift < 0)
                            {
                                int caesar = i - shift + alphabetLower.Length;
                                decryptedText += alphabetLower[caesar];
                                break;
                            }
                            else
                            {
                                int caesar = i - shift;
                                decryptedText += alphabetLower[caesar];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < alphabetUpper.Length; i++)
                    {
                        if (character == alphabetUpper[i])
                        {
                            if (i - shift < 0)
                            {
                                int caesar = i - shift + alphabetUpper.Length;
                                decryptedText += alphabetUpper[caesar];
                                break;
                            }
                            else
                            {
                                int caesar = i - shift;
                                decryptedText += alphabetUpper[caesar];
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                decryptedText += character;
            }
        }
        return decryptedText;
    }

    public BigInteger phi(BigInteger a, BigInteger b)
    {
        BigInteger phiN = (a - 1) * (b - 1);
        return phiN;
    }

    public string EncryptTextWithPublicKey(BigInteger e, BigInteger N, string data)
    {
        data = data.Trim();

        string[] args = data.Split();

        string t = "";

        for(int i = 0; i < args.Length; i++)
        {
            char[] c = args[i].ToCharArray();
            for (int z = 0; z < c.Length; z++)
            {
                BigInteger cipher = BigInteger.ModPow(BigInteger.Parse(((int)c[z]).ToString()), e, N);
                t += cipher.ToString();
                t += " ";
            }
        }

        t = t.Trim();

        return t;
    }

    public string DecryptTextWithPrivateKey(BigInteger e, BigInteger N, string data, BigInteger binaryA, BigInteger binaryB)
    {
        BigInteger phiN = phi(binaryA, binaryB);
        BigInteger d = ModInverse(e, phiN);

        data = data.Trim();

        string[] args = data.Split();

        string t = "";

        for (int i = 0; i < args.Length; i++)
        {
                BigInteger cipher = BigInteger.ModPow(BigInteger.Parse(args[i]),d,N);
                t += (char)cipher;
         
        }
        t += " ";
        t = t.Trim();

        return t;
    }

    public BigInteger ModInverse(BigInteger e, BigInteger phi_n)
    {
        BigInteger d = 1;
        while (true)
        {
            if ((d * e) % phi_n == 1)
                return d;
            d++;
        }
    }

    public string VigenereEncrypt(string text, string key)
    {
        string encryptedText = "";

        int keyCounter = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if(char.IsLetter(text[i]))
            {
                int keyValue = keyCounter % key.Length;
                int textValue = 0;


                for (int t = 0; t < alphabetLower.Length; t++)
                {
                    if (text[i] == alphabetLower[t])
                    {
                        textValue = t;
                        break;
                    }
                }

                Debug.Log(key[keyValue] + " " + alphabetLower[textValue]);


                for (int k = 0; k < alphabetLower.Length; k++)
                {
                    if (key[keyValue] == alphabetLower[k])
                    {
                        keyValue = k;
                        break;
                    }
                }

                int final = (keyValue + textValue) % alphabetLower.Length;

                encryptedText += alphabetLower[final];

                keyCounter += 1;
            }
            else
            {
                encryptedText += text[i];
            }
        }
        return encryptedText;
    }

    public string VigenereDecrypt(string text , string key)
    {
        string decryptedText = "";

        int keyCounter = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsLetter(text[i]))
            {
                int keyValue = keyCounter % key.Length;
                int textValue = 0;

                for (int t = 0; t < alphabetLower.Length; t++)
                {
                    if (text[i] == alphabetLower[t])
                    {
                        textValue = t;
                        break;
                    }
                }

                Debug.Log(key[keyValue] + " " + alphabetLower[textValue]);


                for (int k = 0; k < alphabetLower.Length; k++)
                {
                    if (key[keyValue] == alphabetLower[k])
                    {
                        keyValue = k;
                        break;
                    }
                }

                int final = (textValue - keyValue) % alphabetLower.Length;

                if(textValue - keyValue >= 0)
                {
                    decryptedText += alphabetLower[final];
                }
                else
                {
                    decryptedText += alphabetLower[alphabetLower.Length + final];
                }

                keyCounter += 1;
            }
            else
            {
                decryptedText += text[i];
            }
        }
        return decryptedText;
    }

    public void VigenereEncryptVoid(string key)
    {
        text = "";
        counter = 0;
        textThatWannaCrypted = VigenereEncrypt(textThatWannaCrypted,key);
        textChars = textThatWannaCrypted.ToCharArray();
    }
    public void VigenereDecryptVoid(string key)
    {
        text = "";
        counter = 0;
        textThatWannaCrypted = VigenereDecrypt(textThatWannaCrypted,key);
        textChars = textThatWannaCrypted.ToCharArray();
    }

    public string stringToByteText(string text,int spacing)
    {
        char[] chars = text.ToCharArray();
        string t = "";

        foreach(char c in chars)
        {
            t += Convert.ToByte(c);

            for (int i = 0; i < spacing; i++)
            {
                t += " ";
            }
        }

        t.Trim();

        return t;
    }
}
