using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalManager : MonoBehaviour
{
    public GameObject directoryLine;
    public GameObject responseLine;

    public TMP_InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;


    Interpreter interpreter;

    private void Start()
    {
        interpreter = GetComponent<Interpreter>();
    }

    private void OnGUI()
    {
        if(terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            string userInput = terminalInput.text;

            ClearInputField();

            AddDirectoryLine(userInput);

            int lines = AddInterpreterLines(interpreter.Interpret(userInput));

            userInputLine.transform.SetAsLastSibling();

            terminalInput.ActivateInputField();

            terminalInput.Select();

            sr.normalizedPosition = new Vector2(0, 0);
        }
    }

    void ClearInputField()
    {
        terminalInput.text = "";
    }

    void AddDirectoryLine(string userInput)
    {
        GameObject msg = Instantiate(directoryLine, msgList.transform);

        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;

        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 35.0f);

        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        msg.GetComponentsInChildren<TextMeshProUGUI>()[1].text = userInput;
    }

    int AddInterpreterLines(List<string> interpretation)
    {
        for(int i = 0; i<interpretation.Count; i++)
        {
            GameObject res = Instantiate(responseLine, msgList.transform);

            res.transform.SetAsLastSibling();

            Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;

            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 35.0f);

            res.GetComponentInChildren<TextMeshProUGUI>().text = interpretation[i];
        }

        return interpretation.Count;
    }
}
