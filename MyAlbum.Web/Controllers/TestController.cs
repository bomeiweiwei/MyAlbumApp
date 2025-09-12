using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Application.EmployeeAccount;
using MyAlbum.Application.Identity;
using MyAlbum.Application.Identity.implement;
using MyAlbum.Application.Test;
using MyAlbum.Models.Employee;
using MyAlbum.Models.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAlbum.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ITestService _testService;
        private readonly IIdentityService _identityService;
        public TestController(IWebHostEnvironment env, ITestService testService, IIdentityService identityService)
        {
            _env = env;
            _testService = testService;
            _identityService = identityService;
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
        [Route("TestLogin")]
        public async Task<IActionResult> TestLogin(LoginReq req)
        {
            var dto = await _identityService.Login(req);
            if (dto is null) return NotFound();
            return Ok(dto);
        }
    }
}

