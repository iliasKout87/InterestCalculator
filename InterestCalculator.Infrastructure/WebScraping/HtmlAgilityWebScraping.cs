using InterestCalculator.Models.Types;
using InterestCalculator.Persistence.WebScraping;
using HtmlAgilityPack;
using System.Globalization;
using System.Net;

namespace InterestCalculator.Infrastructure.WebScraping;

public class HtmlAgilityWebScraping : IWebScraping
{
    public async Task<BankInterestRateSchedule> GetBankInterestRateScheduleAsync(string url)
    {
        var handler = new HttpClientHandler 
        {
            AutomaticDecompression = DecompressionMethods.All,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        using var client = new HttpClient(handler);

        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.Add("Connection", "keep-alive");

        await client.GetAsync("https://www.bankofgreece.gr/");
        var response = await client.GetAsync("https://www.bankofgreece.gr/statistika/xrhmatopistwtikes-agores/ekswtrapezika-epitokia");
        //response.EnsureSuccessStatusCode();

        var htmlContent = await response.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);


        //TODO: handle not found
        //TODO: add to settings
        var bankInterestRateTable = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='insuranceCompany__table']");

        var bankInterestRateTableRows = bankInterestRateTable.SelectNodes(".//tr");

        var bankInterestRates = new List<BankInterestRate>();

        //TODO: handle failure to find
        foreach (var row in bankInterestRateTableRows)
        {
            var rowCells = row.SelectNodes(".//td");

            if (rowCells == null || rowCells.Count == 0)
            {
                continue;
            }

            var dateFromCell = rowCells[0];
            var dateFromCellValue = dateFromCell.InnerText;

            var dateToCell = rowCells[1];
            var dateToCellValue = dateToCell.InnerText;

            var bankInterestRateCell = rowCells[4];
            var bankInterestRateCellValue = bankInterestRateCell.InnerText
                .Substring(0, bankInterestRateCell.InnerText.Length - 1)
                .Replace(',', '.');

            //TODO:use try parse and handle failure
            var bankInterestRate = new BankInterestRate(
                new InterestRate(decimal.Parse(bankInterestRateCellValue, CultureInfo.InvariantCulture)/100),
                new DatePeriod(
                    DateOnly.ParseExact(dateFromCellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    row == bankInterestRateTableRows[bankInterestRateTableRows.Count - 1]
                        ? DateOnly.FromDateTime(DateTime.Today)
                        : DateOnly.ParseExact(dateToCellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                )
            );

            bankInterestRates.Add(bankInterestRate);
        }

        return BankInterestRateSchedule.Create(bankInterestRates);
    }
}

