using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GameEventDetailPanel : MonoBehaviour
{
    public Text myText;

    public void UpdateDescription(string content)
    {
        myText.text = content;
    }
}
