namespace CrossInform.Triplets;

/// <summary>
/// Класс для последовательного поиска триплетов с использованием <see cref="Span{T}"/>.
/// </summary>
internal sealed class SequentialSpanTripletsFinder : TripletsFinder
{
    private readonly Dictionary<string, int> _triplets = new();

    /// <inheritdoc/>
    public SequentialSpanTripletsFinder(string file) : base(file)
    {
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, int> Triplets => _triplets;

    /// <inheritdoc/>
    protected override void Process()
    {
        var text = new string(Buffer).ToLowerInvariant();
        var span = text.AsSpan();

        for (var index = StartIndex; index < EndIndex; index++)
        {
            var triplet = CreateTriplet(span, index);

            if (AreLetters(triplet))
            {
                IncreaseTripletCount(triplet.ToString());
            }
        }
    }

    private void IncreaseTripletCount(string triplet)
    {
        if (_triplets.TryGetValue(triplet, out var value))
        {
            _triplets[triplet] = ++value;
        }
        else
        {
            _triplets.Add(triplet, 1);
        }
    }
}
