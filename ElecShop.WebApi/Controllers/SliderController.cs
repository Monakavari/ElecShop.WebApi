﻿using ElecShop.WebApi.Core.Services.Contracts;
using ElecShop.WebApi.Core.Utilities.Common;
using Microsoft.AspNetCore.Mvc;

namespace ElecShop.WebApi.Controllers
{
    public class SliderController : SiteBaseController
    {
        #region constructor

        private ISliderService sliderService;

        public SliderController(ISliderService sliderService)
        {
            this.sliderService = sliderService;
        }

        #endregion

        #region all active sliders

        [HttpGet("GetActiveSliders")]
        public async Task<IActionResult> GetActiveSliders()
        {
            var sliders = await sliderService.GetActiveSliders();

            return JsonResponseStatus.Success(sliders);
        }

        #endregion
    }
}
