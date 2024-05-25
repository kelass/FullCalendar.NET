using Calendar.Models.DtoModels;
using Calendar.Options.Google;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Calendar.BL.Services.HolidayService
{
    public class HolidayService : IHolidayService
    {
        private Holiday holidays;
        private readonly GoogleCalendarOptions _googleCalendarOptions;
        public HolidayService(IOptions<GoogleCalendarOptions> googleCalendarOptions)
        {
            _googleCalendarOptions = googleCalendarOptions.Value;
        }

        public async Task<IEnumerable<HolidayDto>> GetSwedishHolidaysFromAPI()
        {
            string apiUrl = $"{_googleCalendarOptions.GoogleEndpoint}{_googleCalendarOptions.GoogleCountry}/events?key={_googleCalendarOptions.ApiKey}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        holidays = JsonConvert.DeserializeObject<Holiday>(json);
                    }
                    else
                    {
                        Console.WriteLine("Exception api:" + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception" + ex.Message);
                }
            }
            
            return holidays.Items.Select(item => new HolidayDto
            {
                Title = item.Summary,
                Start = item.Start.Date,
                End = item.End.Date
            }).ToList();
        }

        #region private
        private class Holiday
        {
            [JsonProperty("items")]
            public IEnumerable<Item> Items { get; set; }
        }

        private class Item
        {
            [JsonProperty("start")]
            public JsonDate Start { get; set; }
            [JsonProperty("end")]
            public JsonDate End { get; set; }
            [JsonProperty("summary")]
            public string Summary { get; set; }
        }
        private class JsonDate
        {
            [JsonProperty("date")]
            public string Date { get; set; }
        }
        #endregion
    }
}
