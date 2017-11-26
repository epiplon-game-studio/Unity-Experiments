using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class DropArea : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		var background = GetComponent<Image>();

		var dropHandler = GetComponent<ObservableDropTrigger>().OnDropAsObservable()
			.Subscribe(e =>
			{
				Debug.Log("Drop");
				if (e.selectedObject != null)
				{
					Debug.Log("Drop object: " + e.selectedObject.name);
					e.selectedObject.transform.SetParent(transform, worldPositionStays: true);
					e.selectedObject = null;
				}
			});

		var pointerEnter = GetComponent<ObservablePointerEnterTrigger>().OnPointerEnterAsObservable()
			.Subscribe(e => background.color = Color.green);
		var pointerExit = GetComponent<ObservablePointerExitTrigger>().OnPointerExitAsObservable()
			.Subscribe(e => background.color = Color.white);
	}
}
