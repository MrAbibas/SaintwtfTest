using UnityEngine;
using UnityEngine.UI;
public class FactoryView : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private ResourceCountView[] countViews;
    internal void UpdateProgres(float progres)
    {
        image.fillAmount = progres;
    }
    internal void SetResourceCount(ResourceType type, int count)
    {
        for (int i = 0; i < countViews.Length; i++)
            if (countViews[i].resourceType == type)
                countViews[i].Count = count;
    }
}