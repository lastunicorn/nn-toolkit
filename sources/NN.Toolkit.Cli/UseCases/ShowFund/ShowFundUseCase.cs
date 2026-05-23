using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.NN.Toolkit.Cli.DataAccess;
using DustInTheWind.NN.Toolkit.Cli.Domain;

namespace DustInTheWind.NN.Toolkit.Cli.UseCases.ShowFund;

internal class ShowFundUseCase : IUseCase
{
    private readonly IUnitOfWork unitOfWork;

    public ShowFundUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void Execute()
    {
        IEnumerable<FundRecord> fundRecords = unitOfWork.FundRecordRepository.GetAll();

        DisplayFundRecords(fundRecords);
    }

    private void DisplayFundRecords(IEnumerable<FundRecord> fundRecords)
    {
        DataGrid dataGrid = new();

        dataGrid.Columns.Add("Date", HorizontalAlignment.Center);
        dataGrid.Columns.Add("Value", HorizontalAlignment.Right);

        foreach (FundRecord fundRecord in fundRecords)
        {
            dataGrid.Rows.Add(
                fundRecord.Date,
                fundRecord.Value);
        }

        dataGrid.Display();
    }
}