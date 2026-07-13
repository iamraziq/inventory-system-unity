using UnityEngine;
using TMPro;

public class TooltipView : MonoBehaviour
{
    [SerializeField] private GameObject root; // the tooltip panel(hidden by default)
    [SerializeField] private TMP_Text label;

    public void Show(string text)
    {
        label.text = text;
        root.SetActive(true);
    }

    public void Hide() => root.SetActive(false);
}
