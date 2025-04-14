using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteAlways]
public class TextWithDots : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private string _leftText = "Left";
    [SerializeField] private string _rightText = "Right";
    [SerializeField] private char _fillChar = '.';

    [Header("Colors")]
    public Color leftTextColor = Color.white;
    public Color rightTextColor = Color.white;

    public string leftText
    {
        get => _leftText;
        set { _leftText = value; UpdateText(); }
    }

    public string rightText
    {
        get => _rightText;
        set { _rightText = value; UpdateText(); }
    }

    public char fillChar
    {
        get => _fillChar;
        set { _fillChar = value; UpdateText(); }
    }

    private TextMeshProUGUI tmp;
    private Vector2 lastRectSize;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    void Update()
    {
        Vector2 currentRectSize = tmp.rectTransform.rect.size;
        if (currentRectSize != lastRectSize)
        {
            lastRectSize = currentRectSize;
            UpdateText();
        }
    }

    void UpdateText()
    {
        if (!tmp) return;

        tmp.ForceMeshUpdate();

        // Get the size of the text
        float totalWidth = tmp.rectTransform.rect.width;
        float leftWidth = tmp.GetPreferredValues(_leftText).x;
        float rightWidth = tmp.GetPreferredValues(_rightText).x;
        float fillCharWidth = tmp.GetPreferredValues(_fillChar.ToString()).x;

        // Calculate the number of periods to place
        int fillCharCount = Mathf.FloorToInt((totalWidth - leftWidth - rightWidth) / fillCharWidth);
        fillCharCount = Mathf.Max(0, fillCharCount);

        string dots = new string(_fillChar, fillCharCount);

        string leftColorHex = ColorUtility.ToHtmlStringRGBA(leftTextColor);
        string rightColorHex = ColorUtility.ToHtmlStringRGBA(rightTextColor);

        // Set the text
        tmp.text = $"<color=#{leftColorHex}>{_leftText}</color>{dots}<color=#{rightColorHex}>{_rightText}</color>";
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
