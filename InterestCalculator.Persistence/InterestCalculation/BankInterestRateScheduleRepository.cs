using InterestCalculator.Models.Types;
using InterestCalculator.Application.Persistence;
using InterestCalculator.Persistence.WebScraping;
using Microsoft.Extensions.Options;

namespace InterestCalculator.Persistence.InterestCalculation;

public class BankInterestRateScheduleRepository : IReadOnlyRepository<BankInterestRateSchedule>
{
    private readonly IWebScraping _webScraping;
    private readonly BankInterestRateOptions _options;


    public BankInterestRateScheduleRepository(
        IWebScraping webScraping,
        IOptions<BankInterestRateOptions> options
    )
    {
        _webScraping = webScraping;
        _options = options.Value;
    }


    public Task<BankInterestRateSchedule> GetAsync() => _webScraping.GetBankInterestRateScheduleAsync(_options.BankOfGreeceInterestRateUrl);

}

