using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPBlog.Core.Models.Royalty;
using TPBlog.Core.Models;
using TPBlog.Core.Services;
using TPBlog.Data.SeedWorks;
using static TPBlog.Core.SeedWorks.Contants.Permissions;
using TPBlog.Api.Extensions;
using TPBlog.Data.Services;

namespace TPBlog.Api.Controllers
{
    [Route("api/admin/royalty")]
    [ApiController]
    public class RoyaltyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoyaltyService _royaltyService;
        public RoyaltyController(IUnitOfWork unitOfWork, IRoyaltyService royaltyService)
        {
            _royaltyService = royaltyService;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("transaction-histories")]
        [Authorize(Royalty.View)]
        public async Task<ActionResult<PageResult<TransactionDto>>> GetTransactionHistory(string? keyword,
          int fromMonth, int fromYear, int toMonth, int toYear,
            int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.IC_Transactions.GetAllPaging(keyword, fromMonth, fromYear, toMonth, toYear, pageIndex, pageSize);
            return Ok(result);
        }
        [HttpGet]
        [Route("Royalty-report-by-user")]
        [Authorize(Royalty.View)]
        public async Task<ActionResult<List<RoyaltyReportByUserDto>>> GetRoyaltyReportByUser(Guid? userId,
          int fromMonth, int fromYear, int toMonth, int toYear)
        {
            var result = await _royaltyService.GetRoyaltyReportByUserAsync(userId, fromMonth, fromYear, toMonth, toYear);
            return Ok(result);
        }

        [HttpGet]
        [Route("Royalty-report-by-month")]
        [Authorize(Royalty.View)]
        public async Task<ActionResult<List<RoyaltyReportByMonthDto>>> GetRoyaltyReportByMonth(Guid? userId,
         int fromMonth, int fromYear, int toMonth, int toYear)
        {
            var result = await _royaltyService.GetRoyaltyReportByMonthAsync(userId, fromMonth, fromYear, toMonth, toYear);
            return Ok(result);
        }

        [HttpPost]
        [Route("{userId}")]
        [Authorize(Royalty.Pay)]
        //public async Task<IActionResult> PayRoyalty(Guid userId)
        public async Task<IActionResult> PayRoyalty(Guid userId)
        {
            var fromUserId = User.GetUserId();
            await _royaltyService.PayRoyaltyForUserAsync(fromUserId, userId);
            return Ok();
        }
    }
}
