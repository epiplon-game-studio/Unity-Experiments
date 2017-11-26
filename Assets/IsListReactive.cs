using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class IsListReactive : MonoBehaviour
{

	public Button AddButton;
	public Text CountText;

	ReactiveCollection<int> numbers;

	void Start()
	{
		numbers = new ReactiveCollection<int>();
		numbers.ObserveCountChanged(notifyCurrentCount: true)
			.SubscribeToText(CountText);
		AddButton.OnClickAsObservable().Subscribe(_ => numbers.Add(Random.Range(0, 100)));
	}
}
