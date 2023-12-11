using Coupon.EndPoint.Models;

namespace Coupon.EndPoint.Services.Contracts
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> DeleteCouponAsync(int id);
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto updateCouponDto);
        Task<ResponseDto?> CreateCouponAsync(CouponDto createCouponDto);
        Task<ResponseDto?> GetCouponByCouponCodeAsync(string couponCode);
    }
}
