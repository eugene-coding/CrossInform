namespace CrossInform.Triplets;

/// <summary>
/// Класс для последовательного поиска триплетов.
/// </summary>
internal sealed class SequentialTripletsFinder : TripletsFinder
{
    private readonly Dictionary<string, int> _triplets = new();

    /// <inheritdoc/>
    public SequentialTripletsFinder(string file) : base(file)
    {
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, int> Triplets => _triplets;

    /// <inheritdoc/>
    protected override void Process()
    {
        for (var index = StartIndex; index < EndIndex; index++)
        {
            var triplet = CreateTriplet(index);

            if (triplet.All(char.IsLetter))
            {
                IncreaseTripletCount(triplet);
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
