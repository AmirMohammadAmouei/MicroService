﻿namespace Mango.Services.CouponAPI.Models.DTO
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public string DiscountAmount { get; set; }
        public string MinAmount { get; set; }
    }
}
