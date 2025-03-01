using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.Web.Controllers
{
    public class ShiftController : ControllerBase
    {
        private readonly IShiftTypeService _shiftTypeService;

        public ShiftController(IShiftTypeService shiftTypeService)
        {
            _shiftTypeService = shiftTypeService;
        }
        /// <summary>
        /// Security manager gets all shift type.
        /// </summary>
        [Route("shift-types")]
        [HttpGet]
        public async Task<IActionResult> GetShiftTypes()
        {
            var shiftTypes = await _shiftTypeService.GetAllShiftTypesAsync();
            return StatusCode(shiftTypes.StatusCode, shiftTypes);
        }

        /// <summary>
        /// Security manager creates a new shift type.
        /// </summary>
        /// <param name="shiftTypeDTO" >Lưu ý khi nhập thông tin time vào nhớ định dạng hh:mm:ss
        /// (12:30:05 hay 05:45:12) chứ nhập 12:30 hay 5:45:12 hay 24:00:00 thay vì 00:00:00 lỗi ráng chịu</param>
        [Route("create-shift-type")]
        [HttpPost]
        public async Task<IActionResult> CreateShiftType([FromBody] ShiftTypeDTO shiftTypeDTO)
        {
            var result = await _shiftTypeService.CreateShiftTypeAsync(shiftTypeDTO);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Security manager updates a shift type.
        /// </summary>
        /// <param name="shiftTypeDTO" >Lưu ý khi nhập thông tin time vào nhớ định dạng hh:mm:ss
        /// (12:30:05 hay 05:45:12) chứ nhập 12:30 hay 5:45:12 hay 24:00:00 thay vì 00:00:00 lỗi ráng chịu</param>
        [Route("update-shift-type/{typeId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateShiftType(Guid typeId, [FromBody] ShiftTypeDTO shiftTypeDTO)
        {
            var result = await _shiftTypeService.UpdateShiftTypeAsync(typeId, shiftTypeDTO);
            return StatusCode(result.StatusCode, result);
        }

        [Route("delete-shift-type/{typeId}")]
        [HttpPut]
        public async Task<IActionResult> DeleteShiftType(Guid typeId)
        {
            var result = await _shiftTypeService.DeleteShiftTypeAsync(typeId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
