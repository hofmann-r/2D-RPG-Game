using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T> {
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public ObservableStack(Stack<T> items) : base(items) {}

    public ObservableStack() {}

    public new void Push(T item) {
        base.Push(item);

        if (OnPush != null) {
            OnPush();
        }
    }

    public new T Pop() {
        T item = base.Pop();

        if (OnPop != null) {
            OnPop();
        }

        return item;
    }

    public new void Clear() {
       base.Clear();

        if (OnClear != null) {
            OnClear();
        }
    }
}
