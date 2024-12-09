namespace Pokorm.AdventOfCode.Tests.Logging;

internal sealed class XUnitLogScope(object state)
{
    private static readonly AsyncLocal<XUnitLogScope?> value = new AsyncLocal<XUnitLogScope?>();

    public object State { get; } = state;

    internal static XUnitLogScope? Current
    {
        get => value.Value;
        set => XUnitLogScope.value.Value = value;
    }

    internal XUnitLogScope? Parent { get; private set; }

    public override string ToString() => this.State.ToString() ?? string.Empty;

    internal static IDisposable Push(object state)
    {
        var temp = Current;

        Current = new XUnitLogScope(state)
        {
            Parent = temp
        };

        return new DisposableScope();
    }

    private sealed class DisposableScope : IDisposable
    {
        public void Dispose()
        {
            Current = Current?.Parent;
        }
    }
}
