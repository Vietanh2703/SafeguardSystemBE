using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;
using SafeguardSystem.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.Services
{
    public class SecurityshiftService : ISecurityshiftService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SecurityshiftService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> AssignShiftAsync(Guid locationId, Guid teamId, Guid typeId)
        {
            var location = await _unitOfWork.Locations.GetLocationByIdAsync(locationId);
            if (location == null)
            {
                return new ResponseDTO("Location not found", 404, false);
            }

            var team = await _unitOfWork.Teams.GetByGuIdAsync(teamId);
            if (team == null)
            {
                return new ResponseDTO("Team not found", 404, false);
            }

            var type = await _unitOfWork.ShiftTypes.GetByGuIdAsync(typeId);
            if (type == null)
            {
                return new ResponseDTO("Type not found", 404, false);
            }

            var Securityshift = new SecurityShift
            {
                ShiftId = Guid.NewGuid(),
                LocationId = locationId,
                TeamId = teamId,
                TypeId = typeId
            };

            await _unitOfWork.SecurityShifts.AddAsync(Securityshift);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Shift assigned to team successfully", 200, true);
        }
    }
}
