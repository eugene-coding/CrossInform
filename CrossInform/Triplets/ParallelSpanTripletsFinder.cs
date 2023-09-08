using System.Collections.Concurrent;

namespace CrossInform.Triplets;

/// <summary>
/// Класс для параллельного поиска триплетов с использованием <see cref="Span{T}"/>.
/// </summary>
internal sealed class ParallelSpanTripletsFinder : TripletsFinder
{
    private readonly ConcurrentDictionary<string, int> _triplets = new();

    /// <inheritdoc/>
    public ParallelSpanTripletsFinder(string file) : base(file)
    {
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, int> Triplets => _triplets;

    /// <inheritdoc/>
    protected override void Process()
    {
        var text = new string(Buffer).ToLowerInvariant();

        Parallel.For(StartIndex, EndIndex, index =>
        {
            var triplet = CreateTriplet(text.AsSpan(), index);

            if (AreLetters(triplet))
            {
                _triplets.AddOrUpdate(triplet.ToString(), 1, (key, oldValue) => oldValue + 1);
            }
        });
    }
}
