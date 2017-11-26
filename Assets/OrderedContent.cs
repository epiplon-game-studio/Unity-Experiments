using UnityEngine;
using UnityEngine.UI;

public class OrderedContent : MonoBehaviour
{
    public Text Description;

    public string Name;
    public int Value;

    public void Generate()
    {
        for (int i = 0; i < 5; i++)
        {
            Name += (char)Random.Range(0, 100);
        }
        Value = Random.Range(0, 99);
        Description.text = Name + "   <color=red>" + Value.ToString() + "</color>";
    }
}
