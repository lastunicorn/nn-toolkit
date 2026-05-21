namespace DustInTheWind.NN.Toolkit.Pilonul2;

public struct MonthDate
{
    public int Year { get; set; }

    public int Month { get; set; }

    public MonthDate(int year, int month)
    {
        if (year <= 0)
            throw new ArgumentOutOfRangeException("Invalid year.");

        if (month <= 0 || month > 12)
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