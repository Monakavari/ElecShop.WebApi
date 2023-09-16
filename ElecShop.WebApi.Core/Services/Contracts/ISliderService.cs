using ElecShop.WebApi.DataLayer.Entities.Site;

namespace ElecShop.WebApi.Core.Services.Contracts
{
    public interface ISliderService : IDisposable
    {
        Task<List<Slider>> GetAllSliders();
        Task<List<Slider>> GetActiveSliders();
        Task AddSlider(Slider slider);
        Task UpdateSlider(Slider slider);
        Task<Slider> GetSliderById(long sliderId);
    }
}
