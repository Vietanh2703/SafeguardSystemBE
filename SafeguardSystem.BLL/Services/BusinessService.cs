using AutoMapper;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.Common.DTOs;
using SafeguardSystem.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeguardSystem.BLL.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusinessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ResponseDTO GetAllBusinessesAsync()
        {
            var businesses =_unitOfWork.Businesses.GetAll();
            if (businesses == null || !businesses.Any())
            {
                return new ResponseDTO("No business partner found", 200, true, null);
            }
            var businessDto = businesses.Select(b => new BusinessDTO
            {
                Name = b.Name,
                Description = b.Description
            }).ToList();
            return new ResponseDTO("Businesses retrieved successfully", 200, true, businesses);
        }
    }
}
