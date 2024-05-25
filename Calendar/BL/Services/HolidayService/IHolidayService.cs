using Calendar.Models.DtoModels;

namespace Calendar.BL.Services.HolidayService
{
    public interface IHolidayService
    {
        Task<IEnumerable<HolidayDto>> GetSwedishHolidaysFromAPI();
    }
}
