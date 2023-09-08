using System.Collections.Concurrent;

namespace CrossInform.Triplets;

/// <summary>
/// Класс для последовательного поиска триплетов.
/// </summary>
internal sealed class ParallelTripletsFinder : TripletsFinder
{
    private readonly ConcurrentDictionary<string, int> _triplets = new();

    /// <inheritdoc/>
    public ParallelTripletsFinder(string file) : base(file)
    {
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, int> Triplets => _triplets;

    /// <inheritdoc/>
    protected override void Process()
    {
        Parallel.For(StartIndex, EndIndex, index =>
        {
            var triplet = CreateTriplet(index);

            if (triplet.All(char.IsLetter))
            {
                _triplets.AddOrUpdate(triplet, 1, (key, oldValue) => oldValue + 1);
            }
        });
    }
}
