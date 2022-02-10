using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    void start()
    {
        textLabel.text = "Hello!";
    }
}
