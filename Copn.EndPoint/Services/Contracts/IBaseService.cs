using Coupon.EndPoint.Models;

namespace Coupon.EndPoint.Services.Contracts
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto request);
    }
}
