using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
	void Start()
	{
		var beginDrag = GetComponent<ObservableBeginDragTrigger>().OnBeginDragAsObservable()
			.Subscribe(e => e.selectedObject = gameObject);

		var onDrag = GetComponent<ObservableDragTrigger>().OnDragAsObservable()
			.Subscribe(e => transform.position = e.position);

		var endDrag = GetComponent<ObservableEndDragTrigger>().OnEndDragAsObservable()
			.Subscribe(e => Debug.Log("End Drag"));
	}
}
