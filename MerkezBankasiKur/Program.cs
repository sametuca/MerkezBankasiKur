using System.Xml.Linq;

namespace MerkezBankasiKur
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient _httpClient = new();
            var currencyList = new List<Currency>();

            var response = await _httpClient.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
            var responseContent = await response.Content.ReadAsStringAsync();

            XDocument doc = XDocument.Parse(responseContent);

            foreach (var currency in doc.Descendants("Currency"))
            {
                currencyList.Add(new Currency()
                {
                    CurrencyCode = (string)currency.Attribute("CurrencyCode"),
                    Name = (string)currency.Element("CurrencyName"),
                    Unit = (int)currency.Element("Unit"),
                    ForexBuying = checkNullOrEmpty(currency.Element("ForexBuying")),
                    ForexSelling = checkNullOrEmpty(currency.Element("ForexSelling")),
                    BanknoteBuying = checkNullOrEmpty(currency.Element("BanknoteBuying")),
                    BanknoteSelling = checkNullOrEmpty(currency.Element("BanknoteSelling"))
                });
            }

            foreach (var item in currencyList)
            {
                Console.WriteLine($"Currency Code: {item.CurrencyCode}");
                Console.WriteLine($"Name: {item.Name}");
                Console.WriteLine($"Unit: {item.Unit}");
                Console.WriteLine($"ForexBuying: {item.ForexBuying}");
                Console.WriteLine($"ForexSelling: {item.ForexSelling}");
                Console.WriteLine($"BanknoteBuying: {item.BanknoteBuying}");
                Console.WriteLine($"BanknoteSelling: {item.BanknoteSelling}");
                Console.WriteLine("-------------------------------------------------");
            }

        }

        private static double checkNullOrEmpty(XElement number)
        {
            return string.IsNullOrEmpty((string)number) ? 0.00 : (double)number;
        }

        public class Currency
        {
            public int CurrencyId { get; set; }
            public string CurrencyCode { get; set; }
            public string Name { get; set; }
            public int Unit { get; set; }
            public double? ForexBuying { get; set; }
            public double? ForexSelling { get; set; }
            public double? BanknoteBuying { get; set; }
            public double? BanknoteSelling { get; set; }

        }
    }
}
