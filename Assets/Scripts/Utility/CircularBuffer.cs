using System;
using System.Collections;
using System.Collections.Generic;

public class CircularBuffer<T> : IEnumerable<T>
{
    private T[] buffer;
    private int head;
    private int size;

    public CircularBuffer(int size)
    {
        buffer = new T[size];
    }

    public void Add(T item)
    {
        buffer[head] = item;
        head = (head + 1) % buffer.Length;
        size = Math.Min(buffer.Length, size + 1);
    }

    public int Size()
    {
        return size;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T item in buffer)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}