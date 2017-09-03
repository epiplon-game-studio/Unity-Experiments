using UnityEngine;

public interface IAction
{
				void Do();
}


public class Move : IAction
{
				public void Do()
				{
								Debug.Log("Object moved");
				}
}

public class Steer : IAction
{
				public void Do()
				{
								Debug.Log("Steering");
				}
}
