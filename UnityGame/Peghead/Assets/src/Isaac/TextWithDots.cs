using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteAlways]
public class TextWithDots : MonoBehaviour
{
    [Header("Text Settings")]
    public string leftText = "Left";
    public string rightText = "Right";
    public char fillChar = '.';

    [Header("Colors")]
    public Color leftTextColor = Color.white;
    public Color rightTextColor = Color.white;

    private TextMeshProUGUI tmp;
    private Vector2 lastRectSize;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    void Update()
    {
        // Detect changes in RectTransform size
        Vector2 currentRectSize = tmp.rectTransform.rect.size;
        if (currentRectSize != lastRectSize)
        {
            lastRectSize = currentRectSize;
            UpdateText();
        }
    }

    void UpdateText()
    {
        tmp.ForceMeshUpdate();

        float totalWidth = tmp.rectTransform.rect.width;

        float leftWidth = tmp.GetPreferredValues(leftText).x;
        float rightWidth = tmp.GetPreferredValues(rightText).x;
        float fillCharWidth = tmp.GetPreferredValues(fillChar.ToString()).x;

        int fillCharCount = Mathf.FloorToInt((totalWidth - leftWidth - rightWidth) / fillCharWidth);
        fillCharCount = Mathf.Max(0, fillCharCount);

        string dots = new string(fillChar, fillCharCount);

        // Serialize colors to HTML hex
        string leftColorHex = ColorUtility.ToHtmlStringRGBA(leftTextColor);
        string rightColorHex = ColorUtility.ToHtmlStringRGBA(rightTextColor);

        // Final formatted text with colors
        tmp.text = $"<color=#{leftColorHex}>{leftText}</color>{dots}<color=#{rightColorHex}>{rightText}</color>";
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!tmp) tmp = GetComponent<TextMeshProUGUI>();
        lastRectSize = tmp.rectTransform.rect.size;
        UpdateText();
    }
#endif
}
