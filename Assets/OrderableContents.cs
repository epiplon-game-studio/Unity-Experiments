using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class OrderableContents : MonoBehaviour
{
    public OrderedContent Prefab;
    public RectTransform Content;
	public HorizontalScrollSnapMultiple scrollSnap;

    List<OrderedContent> cachedContent;

	private void Start()
    {
        cachedContent = new List<OrderedContent>();
        for (int i = 0; i < 10; i++)
        {
            var cont = Instantiate(Prefab, Content.transform, worldPositionStays: false);
			cont.Generate();
			scrollSnap.AddChild(cont.gameObject);

            //cachedContent.Add(cont);
        }
    }

    public void OrderbyValue()
    {
        cachedContent = cachedContent.OrderBy(c => c.Value).ToList();
        foreach (var item in cachedContent)
        {
            item.transform.SetSiblingIndex(cachedContent.IndexOf(item));
        }
    }

    public void OrderbyName()
    {
        cachedContent = cachedContent.OrderBy(c => c.Name).ToList();
        foreach (var item in cachedContent)
        {
            item.transform.SetSiblingIndex(cachedContent.IndexOf(item));
        }
    }
}
