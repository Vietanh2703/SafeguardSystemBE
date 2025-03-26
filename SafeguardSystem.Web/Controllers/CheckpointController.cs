using Microsoft.AspNetCore.Mvc;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace SafeguardSystem.Web.Controllers
{
    public class CheckpointController : ControllerBase
    {
        private readonly ICheckpointService _checkpointService;
        public CheckpointController(ICheckpointService checkpointService)
        {
            _checkpointService = checkpointService;
        }
        
        [Route("{locationId}/checkpoints")]
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve all checkpoints by location",
            Description = "Fetches a list of all registered checkpoints in the system by location.")]
        [SwaggerResponse(200, "Successfully retrieved checkpoints.")]
        [SwaggerResponse(400, "Invalid request data.")]
        [SwaggerResponse(500, "Internal server error.")]
        public async Task<IActionResult> GetCheckpointsByLocationId(Guid locationId)
        {
            var checkpoints = await _checkpointService.GetCheckpointsByLocationIdAsync(locationId);
            return StatusCode(checkpoints.StatusCode, checkpoints);
        }


        [Route("checkpoint")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new checkpoint",
            Description = "Creates a new checkpoint based on the provided information.")]
        [SwaggerResponse(200, "Checkpoint created successfully.")]
        public async Task<IActionResult> CreateCheckpoint([FromBody] CheckpointDTO checkpointDTO)
        {
            var result = await _checkpointService.AddCheckpointAsync(checkpointDTO);
            return StatusCode(result.StatusCode, result);
        }


        [Route("checkpoint")]
        [HttpPut]
        [SwaggerOperation(Summary = "Update a checkpoint",
            Description = "Updates an existing checkpoint based on the provided information.")]
        [SwaggerResponse(200, "Checkpoint updated successfully.")]
        [SwaggerResponse(404, "Checkpoint not found.")]
        public async Task<IActionResult> UpdateCheckpoint(Guid CheckpointId,[FromBody] UpdateCheckpointDTO checkpointDTO)
        {
            var checkpoint = await _checkpointService.UpdateCheckpointAsync(CheckpointId,checkpointDTO);
            return StatusCode(checkpoint.StatusCode, checkpoint);
        }


        [Route("checkpoint/{id}")]
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete a checkpoint",
            Description = "Deletes an existing checkpoint based on the provided information.")]
        [SwaggerResponse(200, "Checkpoint deleted successfully.")]
        [SwaggerResponse(404, "Checkpoint not found.")]
        public async Task<IActionResult> DeleteCheckpoint(Guid id)
        {
            var checkpoint = await _checkpointService.DeleteCheckpointAsync(id);
            return StatusCode(checkpoint.StatusCode, checkpoint);
        }
    }
}
