using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Pilonul2;

namespace DustInTheWind.NN.Toolkit.Cli;

internal class Program
{
    static void Main(string[] args)
    {
        ContributionsDocument document = ContributionsDocument.LoadFromFile(Path.Combine("Data", "contributions.pdf"));

        DataGrid dataGrid = new();

        dataGrid.Columns.Add("Month");
        dataGrid.Columns.Add("Gross Value");
        dataGrid.Columns.Add("Administration Fee");
        dataGrid.Columns.Add("Net Value");
        dataGrid.Columns.Add("Unit Value");
        dataGrid.Columns.Add("Unit Count");
        dataGrid.Columns.Add("Paid in Month");

        //List<string> headerFields = new List<string>();

        using StreamWriter output = new("NN_transactions.csv");
        output.WriteLine("Security Name,Ticker Symbol,Date,Time,Value,Shares,Type,Fees,Note");

        using StreamWriter cashOutput = new("NN_cash_transactions.csv");
        cashOutput.WriteLine("Type,Cash Account,Date,Time,Value,Note");

        foreach (Contribution contribution in document)
        {
            dataGrid.Rows.Add(contribution.Month, contribution.GrossValue, contribution.AdministrationFee, contribution.NetValue, contribution.UnitValue, contribution.UnitCount,
                contribution.PaidInMonth);

            //var paid = cols[6].Split('/');
            string date = $"{contribution.PaidInMonth.Year:00}-{contribution.PaidInMonth.Month:00}-01";
            //var note = string.Join("; ", headerFields.Zip(cols, (h, v) => $"{h}={v}"));
            string note = "aaa";

            output.WriteLine($"NN,NN,{date},08:05,{contribution.GrossValue},{contribution.UnitCount},Buy,{contribution.AdministrationFee},\"{note}\"");
            cashOutput.WriteLine($"Deposit,NN,{date},08:00,{contribution.GrossValue},\"Luna: {contribution.Month}\"");
        }

        dataGrid.Display();
    }
}