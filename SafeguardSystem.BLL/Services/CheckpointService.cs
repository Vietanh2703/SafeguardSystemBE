using Microsoft.Extensions.Configuration;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem.BLL.Services;

public class CheckpointService : ICheckpointService
{
        private readonly IUnitOfWork _unitOfWork;

        public CheckpointService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetCheckpointsByLocationIdAsync(Guid locationId)
        {
            var location = await _unitOfWork.Locations.GetLocationByIdAsync(locationId);
            if (location == null) return new ResponseDTO("Location not found", 404);

            var checkpoints = await _unitOfWork.Checkpoints.GetCheckpointsByLocationIdAsync(locationId);
            var checkpointDTOs = checkpoints.Select(c => new CheckpointDTO
            {
                Name = c.Name,
                Description = c.Description,
                LocationId = c.LocationId
            }).ToList();

            return new ResponseDTO("Success", 200, true, checkpointDTOs);
        }

        public async Task<ResponseDTO> GetCheckpointByIdAsync(Guid checkpointId)
        {
            var checkpoint = await _unitOfWork.Checkpoints.GetByGuIdAsync(checkpointId);
            if (checkpoint == null)
            {
                return new ResponseDTO("Checkpoint not found", 404);
            }

            var checkpointDTO = new CheckpointDTO
            {
                Name = checkpoint.Name,
                Description = checkpoint.Description,
                LocationId = checkpoint.LocationId
            };

            return new ResponseDTO("Success", 200, true, checkpointDTO);
        }

        public async Task<ResponseDTO> AddCheckpointAsync(CheckpointDTO checkpointDTO)
        {
            var checkpoint = new Checkpoint
            {
                CheckpointId = Guid.NewGuid(),
                Name = checkpointDTO.Name,
                Description = checkpointDTO.Description,
                LocationId = checkpointDTO.LocationId,
            };

            await _unitOfWork.Checkpoints.AddAsync(checkpoint);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Checkpoint added successfully", 200, true);
        }

        public async Task<ResponseDTO> UpdateCheckpointAsync(Guid checkpointId, UpdateCheckpointDTO checkpointDTO)
        {
            var checkpoint = await _unitOfWork.Checkpoints.GetByGuIdAsync(checkpointId);
            if (checkpoint == null)
            {
                return new ResponseDTO("Checkpoint not found", 404);
            }

            checkpoint.Name = checkpointDTO.Name;
            checkpoint.Description = checkpointDTO.Description;

            await _unitOfWork.SaveChangeAsync();
            return new ResponseDTO("Checkpoint updated successfully", 200, true);
        }

        public async Task<ResponseDTO> DeleteCheckpointAsync(Guid checkpointId)
        {
            var checkpoint = await _unitOfWork.Checkpoints.GetByGuIdAsync(checkpointId);
            if (checkpoint == null)
            {
                return new ResponseDTO("Checkpoint not found", 404);
            }

            await _unitOfWork.Checkpoints.DeleteAsync(checkpointId);

            return new ResponseDTO("Checkpoint deleted successfully", 200, true); 
        }

}
