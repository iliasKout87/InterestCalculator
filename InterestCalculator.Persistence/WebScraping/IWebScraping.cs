using InterestCalculator.Models.Types;

namespace InterestCalculator.Persistence.WebScraping;

public interface IWebScraping
{
	Task<BankInterestRateSchedule> GetBankInterestRateScheduleAsync(string url);
}

