using System.Collections;
using System.Collections.Generic;

/// <summary>
///  You can use this class to create your own capped List or stack or whatever.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CappedQueue<T> : IEnumerable, IEnumerable<T>
{
    public int MaxSize { get; set; }
    private readonly Queue<T> _queue = new Queue<T>();

    public CappedQueue(int maxSize)
    {
        this.MaxSize = maxSize;
    }


    public IEnumerator<T> GetEnumerator()
    {
        return this._queue.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Returns false if failed to add.
    /// </summary>
    /// <returns>Returns false if failed to add.</returns>
    public bool Enqueue(T item, bool removeItemIfFull = false)
    {
        if (!removeItemIfFull && this.Count >= this.MaxSize)
            return false;
        while (removeItemIfFull && this.Count >= this.MaxSize)
            this.Dequeue();
        this._queue.Enqueue(item);
        return true;
    }

    public T Peek()
    {
        return this._queue.Peek();
    }

    public T Dequeue()
    {
        return this._queue.Dequeue();
    }

    public void Clear()
    {
        this._queue.Clear();              
    }

    public bool Contains(T item)
    {
        return this._queue.Contains(item);
    }

    public Queue<T> GetCopy()
    {
        return new Queue<T>(this._queue);
    }

    public int Count { get { return this._queue.Count; } }
}
