using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{

    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private IMapper _mapper;
        private readonly AppDbContext _context;
        private ResponseDto _response;

        public CouponAPIController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _context.Coupons.ToList();

                if (!objList.Any())
                {
                    _response.Message = "No data was inserted.";
                }
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpGet("{id}")]
        public ResponseDto GetById(int id)
        {
            try
            {
                Coupon? obj = _context.Coupons.FirstOrDefault(x => x.Id == id);

                if (obj == null)
                {
                    _response.Message = "data not found.";
                }

                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(obj);
                _context.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(obj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;

        }


        [HttpPost("{id}")]
        // [EnableCors("OriginPolicy")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon obj = _context.Coupons.FirstOrDefault(x => x.Id == id);
                _context.Coupons.Remove(obj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
