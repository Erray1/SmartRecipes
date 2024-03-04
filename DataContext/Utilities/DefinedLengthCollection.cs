using System.Collections;

namespace SmartRecipes.Server.DataContext.Utilities;

public class DefinedLengthCollection<T> : ICollection<T>
{
    public DefinedLengthCollection(int length)
    {
		Length = length;
		Items = new();
    }
    public List<T> Items { get; init; }
	private int Length { get; init; }
	public int Count => Items.Count;

	public bool IsReadOnly => false;

	public void Add(T item)
	{
		Items.Add(item);
		if (Items.Count > Length)
		{
			Items.RemoveAt(0);
		}
	}

	public void Clear()
	{
		Items.Clear();
	}

	public bool Contains(T item)
	{
		return Items.Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		Items.CopyTo(array, arrayIndex);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return Items.GetEnumerator();
	}

	public bool Remove(T item)
	{
		return Items.Remove(item);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return Items.GetEnumerator();
	}
}
