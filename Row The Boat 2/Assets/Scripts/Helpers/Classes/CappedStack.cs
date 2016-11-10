using System.Collections;
using System.Collections.Generic;

/// <summary>
///  You can use this class to create your own capped List or stack or whatever.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CappedStack<T> : IEnumerable, IEnumerable<T>
{
    public int MaxSize { get; set; }
    private readonly Stack<T> _stack = new Stack<T>();

    public CappedStack(int maxSize)
    {
        this.MaxSize = maxSize;
    }


    public IEnumerator<T> GetEnumerator()
    {
        return this._stack.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Returns false if failed to add.
    /// </summary>
    /// <returns>Returns false if failed to add.</returns>
    public bool Push(T item, bool removeItemIfFull = false)
    {
        if (!removeItemIfFull && this.Count >= this.MaxSize)
            return false;
        while (removeItemIfFull && this.Count >= this.MaxSize)
            this.Pop();
        this._stack.Push(item);
        return true;
    }

    public T Peek()
    {
        return this._stack.Peek();
    }

    public T Pop()
    {
        return this._stack.Pop();
    }

    public void Clear()
    {
        this._stack.Clear();              
    }

    public bool Contains(T item)
    {
        return this._stack.Contains(item);
    }

    public Stack<T> GetCopy()
    {
        return new Stack<T>(this._stack);
    }

    public int Count { get { return this._stack.Count; } }
}
