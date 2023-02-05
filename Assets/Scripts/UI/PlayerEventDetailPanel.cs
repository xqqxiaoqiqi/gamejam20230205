using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEventDetailPanel : MonoBehaviour
{
    public Text myText;

    public void UpdateDescription(string content)
    {
        myText.text = content;
    }
}
