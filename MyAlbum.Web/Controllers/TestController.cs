using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Application.EmployeeAccount.implement;
using MyAlbum.Application.Identity;
using MyAlbum.Application.Identity.implement;
using MyAlbum.Application.MemberAccount;
using MyAlbum.Application.Test;
using MyAlbum.Models;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;
using MyAlbum.Models.Member;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAlbum.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ITestService _testService;
        private readonly IEmployeeAccountCreateService _employeeAccountCreateService;
        private readonly IMemberAccountWriteService _memberAccountWriteService;
        public TestController(IWebHostEnvironment env, ITestService testService, IEmployeeAccountCreateService employeeAccountCreateService, IMemberAccountWriteService memberAccountWriteService)
        {
            _env = env;
            _testService = testService;
            _employeeAccountCreateService = employeeAccountCreateService;
            _memberAccountWriteService = memberAccountWriteService;
        }
        /// <summary>
        /// 取得環境變數
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEnv")]
        public async Task<IActionResult> GetEnv()
        {
            var environment = _env.EnvironmentName;
            return Ok(new { environment });
        }
        /// <summary>
        /// 檢查資料庫連線
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetConnectResult")]
        public async Task<IActionResult> GetConnectResult()
        {
            var connectResult = await _testService.GetConnectResult();
            return Ok(new { connectResult });
        }
        
        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeReq req)
        {
            var accountIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(accountIdStr, out var operatorId))
            {
                req.OperatorId = operatorId;
            }
            var resp = await _employeeAccountCreateService.CreateEmployeeWithAccount(req);
            return Ok(resp.Data);
        }

        [HttpPost]
        [Route("CreateMember")]
        public async Task<IActionResult> CreateMember(CreateMemberReq req)
        {
            var accountIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(accountIdStr, out var operatorId))
            {
                req.OperatorId = operatorId;
            }
            var resp = await _memberAccountWriteService.CreateMemberWithAccountAsync(req);
            return Ok(resp.Data);
        }
    }
}

