using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SerializedStack<T>
{
				[SerializeField, HideInInspector] T[] _array;   // Storage for stack elements
				[SerializeField, HideInInspector] int _size;    // Number of items in the stack.
				[SerializeField, HideInInspector] int _version; // Used to keep enumerator in sync w/ collection.

				private const int _defaultCapacity = 4;
				static T[] _emptyArray = new T[0];

				public SerializedStack()
				{
								_array = _emptyArray;
								_size = 0;
								_version = 0;
				}

				public SerializedStack(int capacity)
				{
								if (capacity < 0)
												throw new ArgumentException("SerializedStack capacity cannot be less than 0", "capacity");

								_array = new T[capacity];
								_size = 0;
								_version = 0;
				}

				public SerializedStack(IEnumerable<T> collection)
				{
								if (collection == null)
												throw new ArgumentNullException("collection");

								ICollection<T> c = collection as ICollection<T>;
								if (c != null)
								{
												int count = c.Count;
												_array = new T[count];
												c.CopyTo(_array, 0);
												_size = count;
								}
								else
								{
												_size = 0;
												_array = new T[_defaultCapacity];

												using (IEnumerator<T> en = collection.GetEnumerator())
												{
																while (en.MoveNext())
																{
																				Push(en.Current);
																}
												}
								}
				}

				public T Peek()
				{
								if (_size == 0)
												throw new InvalidOperationException("Stack is empty");

								return _array[_size - 1];
				}

				public T Pop()
				{
								if (_size == 0)
												throw new InvalidOperationException("Stack is empty");

								_version++;
								T item = _array[--_size];
								_array[_size] = default(T);
								return item;
				}

				public void Push(T item)
				{
								if (_size == _array.Length)
								{
												T[] newArray = new T[(_array.Length == 0) ? _defaultCapacity : 2 * _array.Length];
												Array.Copy(_array, 0, newArray, 0, _size);
												_array = newArray;
								}
								_array[_size++] = item;
								_version++;
				}

				public int Count { get { return _size; } }

				public void Clear()
				{
								Array.Clear(_array, 0, _size);
				}

				public bool Contains(T item)
				{
								int count = _size;

								EqualityComparer<T> c = EqualityComparer<T>.Default;
								while (count-- > 0)
								{
												if (item == null)
												{
																if ((_array[count]) == null)
																				return true;
												}
												else if (_array[count] != null && c.Equals(_array[count], item))
												{
																return true;
												}
								}
								return false;
				}
}
