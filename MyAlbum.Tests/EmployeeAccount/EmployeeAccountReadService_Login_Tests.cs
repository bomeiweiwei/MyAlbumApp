using System;
using FluentAssertions;
using Moq;
using MyAlbum.Application.EmployeeAccount.implement;
using MyAlbum.Domain;
using MyAlbum.Domain.EmployeeAccount;
using MyAlbum.Models;
using MyAlbum.Models.Account;
using MyAlbum.Models.Employee;

namespace MyAlbum.Tests.EmployeeAccount
{
    public class EmployeeAccountReadService_Login_Tests
    {
        private readonly Mock<IEmployeeAccountReadRepository> _repo = new();
        private readonly Mock<IAlbumDbContextFactory> _factory = new();

        private EmployeeAccountReadService CreateSut() =>
            new EmployeeAccountReadService(_factory.Object, _repo.Object);

        [Fact]
        public async Task Login_By_LoginName_Should_Query_Repo_With_Same_Req_And_Token_And_Return_Data()
        {
            // Arrange: 僅使用 LoginName（EmployeeId 不用/為 0）
            var req = new GetEmployeeReq { EmployeeId = 0, LoginName = "admin" };
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            var dto = new AccountDto
            {
                AccountId = Guid.NewGuid(),
                EmployeeId = 1,
                LoginName = "admin",
                FullName = "Admin",
                IsActive = true,
                Email = "admin@example.com",
                PasswordHash = "HASH"
            };
            var expected = new ResponseBase<AccountDto?> { Data = dto, StatusCode = 0, Message = "成功" };

            _repo.Setup(r => r.GetEmployeeAsync(
                    It.Is<GetEmployeeReq>(x => ReferenceEquals(x, req) && x.LoginName == "admin" && x.EmployeeId == 0),
                    It.Is<CancellationToken>(t => t.Equals(token))))
                 .ReturnsAsync(expected);

            var sut = CreateSut();

            // Act
            var actual = await sut.GetEmployeeAsync(req, token);

            // Assert
            actual.Should().BeSameAs(expected);
            _repo.VerifyAll();
        }

        [Fact]
        public async Task Login_By_LoginName_NotFound_Should_Return_Null_Data()
        {
            var req = new GetEmployeeReq { EmployeeId = 0, LoginName = "nobody" };
            _repo.Setup(r => r.GetEmployeeAsync(req, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ResponseBase<AccountDto?> { Data = null, StatusCode = 0, Message = "成功" });

            var sut = CreateSut();

            var result = await sut.GetEmployeeAsync(req, CancellationToken.None);

            result.Data.Should().BeNull();
            _repo.Verify(r => r.GetEmployeeAsync(req, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Login_By_LoginName_Should_Propagate_Exception()
        {
            var req = new GetEmployeeReq { EmployeeId = 0, LoginName = "admin" };
            _repo.Setup(r => r.GetEmployeeAsync(req, It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new InvalidOperationException("DB failed"));

            var sut = CreateSut();

            var act = async () => await sut.GetEmployeeAsync(req, CancellationToken.None);
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("DB failed");
        }
    }
}

