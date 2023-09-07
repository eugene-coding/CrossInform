using CrossInform.Triplets;

using System.Diagnostics;

Stopwatch Stopwatch = new();

var file = GetTxtFile();

Benchmark(new SequentialTripletsFinder(file));
Benchmark(new SequentialSpanTripletsFinder(file));
Benchmark(new ParallelTripletsFinder(file));
Benchmark(new ParallelSpanTripletsFinder(file));

void Benchmark(TripletsFinder tripletsFinder)
{
    Stopwatch.Restart();
    tripletsFinder.FindTriplets();
    Stopwatch.Stop();

    Console.WriteLine($"Название класса: {tripletsFinder.GetType().Name}.");
    Console.WriteLine($"Потребовалось {Stopwatch.ElapsedMilliseconds} мс.");
    Console.WriteLine(tripletsFinder.GetTopTen());
}

static string GetTxtFile()
{
    do
    {
        Console.WriteLine("Введите путь к файлу *.txt");
        var file = Console.ReadLine();

        if (File.Exists(file) && Path.GetExtension(file) == ".txt")
        {
            Console.WriteLine();

            return file;
        }

        Console.WriteLine("Не удалось считать файл\n");

    } while (true);
}
