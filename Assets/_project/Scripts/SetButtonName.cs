using TMPro;
using UnityEngine;

[ExecuteAlways]
public class SetButtonName : MonoBehaviour
{
    private TMP_Text text;

    void Start()
    {
        if (transform.GetComponentInChildren<TMP_Text>() == null) return;
        text = GetComponentInChildren<TMP_Text>();
        text.text = transform.name;
    }
}
