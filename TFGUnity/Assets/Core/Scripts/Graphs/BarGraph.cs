using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarGraph : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform container;

    [Header("Visual Settings")]
    [SerializeField] private Sprite barSprite;
    [SerializeField] private Color barColor;
    [SerializeField] [Range(0.01f, 0.99f)] private float barPercentage;

    [Header("Graph Settings")]
    [SerializeField] DateRange datesRange;
    [SerializeField] int maxEggs;

    Dictionary<int, Bar> barsDictionary;
    float barWidth;
    float barHeightStep;

    public void CreateGraph()
    {
        barHeightStep = container.rect.height / maxEggs;
        barWidth = (container.rect.width / datesRange.GetDaysBetween(true)) * barPercentage;
        float barSeparationWidth = (container.rect.width - barWidth * datesRange.GetDaysBetween(true)) / (datesRange.GetDaysBetween(true) + 1);

        barsDictionary = new Dictionary<int, Bar>();

        for(int i = 0;i < datesRange.GetDaysBetween(true); i++)
        {
            float xPosition = i * barWidth + (i + 1) * barSeparationWidth;
            Bar bar = new Bar(CreateBar(new Vector2(xPosition, 0)), 0);
            barsDictionary.Add(i, bar);
        }
    }

    public void UpdateGraph(Date date)
    {
        int index = datesRange.GetDaysUntil(date);

        barsDictionary[index].amount++;
        barsDictionary[index].rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
        barsDictionary[index].rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, barHeightStep * barsDictionary[index].amount);
    }

    private RectTransform CreateBar(Vector2 anchoredPosition)
    {
        GameObject bar = new GameObject("Bar", typeof(Image));
        bar.transform.SetParent(container, false);
        bar.GetComponent<Image>().sprite = barSprite;
        bar.GetComponent<Image>().color = barColor;
        RectTransform rect = bar.GetComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(barWidth, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.pivot = new Vector2(0, 0);
        return rect;
    }

    private class Bar
    {
        public int amount;
        public RectTransform rect;

        public Bar(RectTransform rect, int amount)
        {
            this.rect = rect;
            this.amount = amount;
        }
    }
}
