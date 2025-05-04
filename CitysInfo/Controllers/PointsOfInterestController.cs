using Asp.Versioning;
using AutoMapper;
using CitysInfo.Entities;
using CitysInfo.Models;
using CitysInfo.Repositories;
using CitysInfo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CitysInfo.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/[controller]")]
    [Authorize(Policy = "MustBeFormAhwaz")]
    [ApiController]
    [ApiVersion(2)]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger
            , IMailService mailService
            , IMapper mapper
            , ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            ////*****
            //var cityClaim = User.Claims.FirstOrDefault(c => c.Type == "city");
            //string cityName = null;

            //if (cityClaim != null)
            //{
            //    cityName = cityClaim.Value;
            //}
            ////*****
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if (!await _cityInfoRepository.CityNameMatchesCityId(cityId, cityName))
            {
                return Forbid();
            }

            //اول باید ببینیم شهر وجود دارد
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                _logger.LogInformation
                    ($"City with if {cityId} wasn't found accessing points of interest");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest
            (int cityId, int pointOfInterestId)
        {
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetByIdForPointOfInterestAsync
                (cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        #region Post
        [HttpPost]
        //[Route("{pointofinterestid:int}")]
        public async Task<ActionResult<PointOfInterestForCreationDto>> CreatePointOfInterest
            (int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterestForCreationDto)
        {
            //یعنی مدل دیتابیس(PointOfInterest) را به
            //PointOfInterestDto تبدیل می‌کنیم تا آن را به کلاینت برگردانیم
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                return NotFound();
            }

            //// For Save To Database
            var finalPointOfInterest =
                _mapper.Map<Entities.PointsOfInterest>(pointOfInterestForCreationDto);

            ////*************اگر اتومپر نبود*************
            //var finalPointOfInterest = new PointsOfInterest
            //{
            //    Name = pointOfInterestForCreationDto.Name,
            //    Description = pointOfInterestForCreationDto.Description
            //};
            ////**************************

            await _cityInfoRepository.AddPointsOfInterestForCityAsync(
                cityId, finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync();

            // For Send To Client
            var createdPointOfInterestToReturn =
                _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            ////*************اگر اتومپر نبود*************
            //var createdPointOfInterestToReturn = new PointOfInterestDto
            //{
            //    Id = finalPointOfInterest.Id,
            //    Name = finalPointOfInterest.Name,
            //    Description = finalPointOfInterest.Description
            //};
            ////**************************


            //////*******اگر فقط بخواهیم شیء ایجادشده را برگردانیم و به مسیر آن نیازی نداریم
            //return Ok(createdPointOfInterestToReturn);

            ////*******اگر بخواهیم هم شی ایجاد شود و هم مسیر ایجاد شود
            // بازگرداندن پاسخ با کد وضعیت 201 (Created)
            return CreatedAtAction(
                nameof(GetPointOfInterest), // نام متد برای دریافت نقطه مورد علاقه
                new // پارامترهای مسیر در Route Values
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn // بدنه پاسخ ، کل جدید
            );
        }
        #endregion

        #region Put
        [HttpPut]
        [Route("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                return NotFound();
            }

            // چرا مستقیماً از GetByIdForPointOfInterestAsync استفاده میکنیم ؟
            //چون بعد از بررسی وجود PointOfInterest، نیاز داریم به اطلاعات آن برای عملیات‌های دیگر مانند put,patch.delete
            var pointOfInterestEntity = await _cityInfoRepository
                .GetByIdForPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //var pointOfId = cityExists.PointsOfInterest
            //    .FirstOrDefault(u => u.Id == pointOfInterestId);
            //if (pointOfId == null)
            //{
            //    return NotFound();
            //}

            ////روش بهتر
            _mapper.Map(pointOfInterestForUpdateDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            //روش اول
            //pointOfId.Name = pointOfInterestForUpdateDto.Name;
            //pointOfId.Description = pointOfInterestForUpdateDto.Description;

            ////روش دوم
            //var updatePoint = new PointOfInterestDto()
            //{
            //    Id = pointOfId.Id,
            //    Name = pointOfInterestForUpdateDto.Name,
            //    Description = pointOfInterestForUpdateDto.Description,
            //};

            //city.PointOfInterest.Remove(pointOfId);
            //city.PointOfInterest.Add(updatePoint);

            return NoContent();
        }
        //#endregion

        //#region Patch
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterestAsync(
        int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            // ۱. بررسی اعتبار ModelState قبل از اعمال تغییرات
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // پیدا کردن شهر
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                return NotFound();
            }

            // پیدا کردن نقطه مورد علاقه
            var pointOfInterestEntity = await _cityInfoRepository
                .GetByIdForPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            // ایجاد یک شیء DTO برای اعمال تغییرات
            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
                pointOfInterestEntity);

            //var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            //{
            //    Name = pointOfInterestEntity.Name,
            //    Description = pointOfInterestEntity.Description,
            //};

            //اعمال تغییرات پچ روی شی DTO
            //و
            //در صورت بروز خطا، خطاها را در modelstate ثبت میکند
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            ////بررسی می‌کنیم که آیا تغییرات ایجاد شده باعث نقض قوانین اعتبارسنجی
            ////(مانند Data Annotations) شده‌اند یا خیر.
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }

            // ۳. اعتبارسنجی دستی مدل
            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(modelState: ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            // اعمال تغییرات به شیء اصلی،بدون ایجاد شی جدید
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent(); // کد وضعیت 204 (No Content)
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Route("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterestAsync(int cityId, int pointOfInterestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //    پیدا کردن شهر
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                return NotFound();
            }

            //    پیدا کردن نقطه مورد علاقه
            var pointOfInterestEntity = await _cityInfoRepository
                .GetByIdForPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            ////اگر از متد دلیت void استفاده میکردیم...
            _cityInfoRepository.DeletePointsOfInterestForCityAsync(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            //await _cityInfoRepository.DeletePointsOfInterestForCityAsync(cityId, pointOfInterestEntity);
            //await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send("Point of interest deleted",
                $"Point if interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");

            return NoContent();
            #endregion
        }
    }
}
