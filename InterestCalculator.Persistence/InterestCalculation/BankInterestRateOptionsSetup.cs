using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace InterestCalculator.Persistence.InterestCalculation;

public class BankInterestRateOptionsSetup : IConfigureOptions<BankInterestRateOptions>
{
	private const string SectionName = "BankInterestRate";
	private readonly IConfiguration _configuration;

	public BankInterestRateOptionsSetup(IConfiguration configuration)
	{
		_configuration = configuration;
	}


	public void Configure(BankInterestRateOptions options) => _configuration.GetSection(SectionName).Bind(options);
}

