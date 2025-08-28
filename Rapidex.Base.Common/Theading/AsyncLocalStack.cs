using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rapidex.Theading;

public class AsyncLocalStack<T> : IEmptyCheckObject
    where T : class
{
    protected AsyncLocal<Stack<T>> localStack;

    public bool IsEmpty => this.localStack.Value == null || this.localStack.Value.Count == 0;
    public int Count => this.localStack.Value?.Count ?? 0;

    public AsyncLocalStack()
    {
        this.localStack = new AsyncLocal<Stack<T>>();
    }

    protected void CheckStack()
    {
        if (this.localStack.Value == null)
            this.localStack.Value = new Stack<T>();
    }

    public void Push(T item)
    {
        item.NotNull("Item can't be null");
        this.CheckStack();
        this.localStack.Value.Push(item);
    }
    public T Pop()
    {
        this.CheckStack();

        if (this.Count == 0)
            return null;

        return this.localStack.Value.Pop();
    }
    public T Peek()
    {
        this.CheckStack();

        if (this.Count == 0)
            return null;

        return this.localStack.Value.Peek();
    }

    public void Clear()
    {
        this.localStack.Value.Clear();
    }
}
