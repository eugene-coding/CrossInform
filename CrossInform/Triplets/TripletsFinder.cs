﻿using System.Text;

namespace CrossInform.Triplets;

/// <summary>
/// Базовый класс для нахождения триплетов в текстовом файле.
/// </summary>
internal abstract class TripletsFinder
{
    /// <summary>
    /// Размер буфера.
    /// </summary>
    /// <remarks>
    /// 81920 - стандартный размер буфера, используемый в <see cref="Stream.CopyTo"/>,
    /// и он находится ниже порога кучи больших объектов (85 кб).
    /// </remarks>
    protected const int BufferSize = 81_920;

    /// <summary>
    /// Значение, на которое нужно сместить буфер 
    /// для хранения двух оставшихся с предыдущей выгрузки символов.
    /// </summary>
    protected const int Offset = 2;

    private const int TripletSize = 3;

    /// <summary>
    /// Создаёт экземпляр класса.
    /// </summary>
    /// <param name="file">Текстовый файл, в котором нужно найти триплеты.</param>
    public TripletsFinder(string file)
    {
        File = file;
    }

    /// <summary>
    /// Буфер для чтения текстового файла по частям.
    /// </summary>
    protected char[] Buffer { get; } = new char[BufferSize];

    /// <summary>
    /// Словарь с триплетами, где ключ - сам триплет, а значение - количество триплетов в <see cref="File"/>.
    /// </summary>
    protected abstract IReadOnlyDictionary<string, int> Triplets { get; }

    /// <summary>
    /// Путь к текстовому файлу, в котором нужно найти триплеты.
    /// </summary>
    protected string File { get; }

    /// <summary>
    /// Находит триплеты в <see cref="File"/> и возвращает <see cref="Triplets"/>,
    /// </summary>
    /// <returns><see cref="Triplets"/>.</returns>
    public IReadOnlyDictionary<string, int> FindTriplets()
    {
        using var reader = new StreamReader(File, Encoding.UTF8);

        while (!reader.EndOfStream)
        {
            ReadBlockIntoBuffer(reader);

            Process();

            AddRemainingLettersToStart();
        }

        return Triplets;
    }

    /// <summary>
    /// Создаёт строку из топ-10 часто встречающихся триплетов.
    /// </summary>
    /// <returns>Строка из топ-10 часто встречающихся триплетов.</returns>
    public string GetTopTen()
    {
        if (Triplets is null)
        {
            return string.Empty;
        }

        var topTen = Triplets.OrderByDescending(pair => pair.Value)
            .Take(10)
            .ToList();

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Топ 10 триплетов:\n");

        foreach (var pair in topTen)
        {
            stringBuilder.Append($"Триплет: {pair.Key}, Количество: {pair.Value}\n");
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Определяет, являются ли все символы в <paramref name="triplet"/> буквами.
    /// </summary>
    /// <param name="triplet">Триплет</param>
    /// <returns>
    /// <see langword="true"/>, если все символы в <paramref name="triplet"/> являются буквами,
    /// иначе - <see langword="false"/>.
    /// </returns>
    protected static bool AreLetters(ReadOnlySpan<char> triplet)
    {
        foreach (var symbol in triplet)
        {
            if (!char.IsLetter(symbol))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Считывает данные в <see cref="Buffer"/>.
    /// </summary>
    /// <param name="reader">Поток, данные из которого необходимо считать.</param>
    protected void ReadBlockIntoBuffer(StreamReader reader)
    {
        reader.ReadBlock(Buffer, Offset, BufferSize - Offset);
    }

    /// <summary>
    /// Создаёт триплет в нижнем регистре на основе <see cref="Buffer"/>.
    /// </summary>
    /// <param name="start">Начальная индекс, от которого нужно создать триплет.</param>
    /// <returns>Созданный триплет.</returns>
    protected string CreateTriplet(int start)
    {
        return new string(Buffer, start, TripletSize).ToLower();
    }

    /// <summary>
    /// Создаёт триплет в нижнем регистре.
    /// </summary>
    /// <param name="span"><see cref="ReadOnlySpan{T}"/>, на основе которого нужно создать триплет.</param>
    /// <param name="start">Начальная индекс, от которого нужно создать триплет.</param>
    /// <returns>Созданный триплет.</returns>
    protected static ReadOnlySpan<char> CreateTriplet(ReadOnlySpan<char> span, int start)
    {
        var triplet = span.Slice(start, TripletSize);
        return triplet;
    }

    /// <summary>
    /// Переносит оставшиеся два символа,на основе которых триплет не построить,
    /// с конца <see cref="Buffer"/> на первые две ячейки.
    /// </summary>
    /// <remarks>
    /// Первые две ячейки <see cref="Buffer>"/> специально зарезервированы 
    /// для оставшихся с предыдущего блока букв.
    /// </remarks>
    protected void AddRemainingLettersToStart()
    {
        Buffer[0] = Buffer[^1];
        Buffer[1] = Buffer[^2];
    }

    /// <summary>
    /// Обрабатывает <see cref="Buffer"/>, чтобы найти триплеты и их количество.
    /// </summary>
    protected abstract void Process();
}