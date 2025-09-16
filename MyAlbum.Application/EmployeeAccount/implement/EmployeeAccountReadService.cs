using System;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;

namespace MyAlbum.Application.EmployeeAccount.implement
{
    public sealed class EmployeeAccountReadService : BaseService, IEmployeeAccountReadService
    {
        private readonly IEmployeeAccountReadRepository _repo;
        public EmployeeAccountReadService(IAlbumDbContextFactory factory, IEmployeeAccountReadRepository repo) : base(factory)
        {
            _repo = repo;
        }

        public async Task<ResponseBase<AccountDto?>> GetEmployeeAsync(GetEmployeeReq req, CancellationToken ct = default)
        {
            return await _repo.GetEmployeeAsync(req, ct);
        }

        public async Task<ResponseBase<List<EmployeeItem>>> GetEmployeeListAsync(PageRequestBase<EmployeeListReq> req, CancellationToken ct = default)
        {
            return await _repo.GetEmployeeListAsync(req, ct);
        }

        public async Task<ResponseBase<EmployeeDto>> GetEmployeeDataByIdAsync(GetEmployeeReq req, CancellationToken ct = default)
        {
            var result = new ResponseBase<EmployeeDto>()
            {
                Data = new EmployeeDto()
            };
            EmployeeDto dto = new EmployeeDto();
            if(req.EmployeeId <= 0)
            {
                result.StatusCode = (long)ReturnCode.DataNotFound;
                result.Message = ReturnCode.DataNotFound.GetDescription();
                return result;
            }

            var empResp = await _repo.GetEmployeeAsync(req, ct);
            if (empResp == null || empResp.StatusCode != (long)ReturnCode.Succeeded)
            {
                result.StatusCode = (long)ReturnCode.DataNotFound;
                result.Message = ReturnCode.DataNotFound.GetDescription();
                return result;
            }
            var data = empResp.Data;
            dto.EmployeeId = data?.EmployeeId ?? 0;
            dto.FullName = data?.FullName ?? string.Empty;
            dto.Title = data?.Title ?? string.Empty;
            dto.HireDate = data?.HireDate ?? null;
            dto.IsActive = data?.IsActive ?? false;
            dto.Email = data?.Email ?? string.Empty;
            result.Data = dto;
            return result;
        }
    }
}

