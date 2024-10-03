using Azure;
using EMedicineBE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Response = EMedicineBE.Models.Response;

namespace EMedicineBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MedicinesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("addToCart")]
        public Response addToCart(Cart cart)
        {
            DAL dal = new DAL();
            SqlConnection  Connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = dal.addToCart(cart, Connection); 
            return response;
        }
        [HttpPost]
        [Route("placeOder")]
        public Response placeOrder(Users users) {
            DAL dal = new DAL();
            SqlConnection Connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = dal.placeOrder(users, Connection);
            return response;

        }

        [HttpGet]
        [Route("orderList")]
        public Response orderList(Users users)
        {
            DAL dal = new DAL();
            SqlConnection Connection = new SqlConnection(_configuration.GetConnectionString("EMedCS").ToString());
            Response response = dal.orderList(users, Connection);
            return response;
        }
    }
}
