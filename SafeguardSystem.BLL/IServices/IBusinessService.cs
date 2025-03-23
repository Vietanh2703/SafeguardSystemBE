using SafeguardSystem.Common.DTOs;

namespace SafeguardSystem.BLL.IServices;

public interface IBusinessService
{
    ResponseDTO GetAllBusinessesAsync();
}