namespace DustInTheWind.NN.Toolkit.P2;

public record struct MonthDate
{
    public int Year { get; private init; }

    public int Month { get; private init; }

    public MonthDate(int year, int month)
    {
        if (year <= 0)
            throw new ArgumentOutOfRangeException("Invalid year.");

        if (month is <= 0 or > 12)
            throw new ArgumentOutOfRangeException("Invalid month.");

        Year = year;
        Month = month;
    }

    public override string ToString()
    {
        return $"{Month:00}/{Year}";
    }

    public static MonthDate Parse(string monthDate)
    {
        string[] parts = monthDate.Split('/');

        return new MonthDate
        {
            Month = int.Parse(parts[0]),
            Year = int.Parse(parts[1])
        };
    }

    public static implicit operator string(MonthDate monthDate)
    {
        return monthDate.ToString();
    }

    public static implicit operator MonthDate(string monthDate)
    {
        return Parse(monthDate);
    }
}