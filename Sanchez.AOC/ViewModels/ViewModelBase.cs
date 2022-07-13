using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace Sanchez.AOC.ViewModels;

public class ViewModelBase : ReactiveObject, IDisposable
{
    protected readonly CompositeDisposable _disposable = new();

    public void Dispose()
    {
        _disposable.Dispose();
    }
}