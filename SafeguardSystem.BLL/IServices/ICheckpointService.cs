using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.IServices;

    public interface ICheckpointService
    {
        Task<ResponseDTO> GetCheckpointsByLocationIdAsync(Guid locationId);
        Task<ResponseDTO> GetCheckpointByIdAsync(Guid checkpointId);
        Task<ResponseDTO> AddCheckpointAsync(CheckpointDTO checkpointDTO);
        Task<ResponseDTO> UpdateCheckpointAsync(Guid checkpointId, UpdateCheckpointDTO checkpointDTO);
        Task<ResponseDTO> DeleteCheckpointAsync(Guid checkpointId);
    }

