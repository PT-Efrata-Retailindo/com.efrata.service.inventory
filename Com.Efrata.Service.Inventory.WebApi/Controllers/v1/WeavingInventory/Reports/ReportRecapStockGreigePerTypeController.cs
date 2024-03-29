﻿using AutoMapper;
using Com.Efrata.Service.Inventory.Lib.Services;
using Com.Efrata.Service.Inventory.Lib.Services.InventoryWeaving.Reports.ReportRecapStockGreigePerType;
using Com.Efrata.Service.Inventory.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Com.Efrata.Service.Inventory.WebApi.Controllers.v1.WeavingInventory.Reports
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/report-recap-stock-type")]
    [Authorize]
    public class ReportRecapStockGreigePerTypeController : Controller
    {

        protected IIdentityService IdentityService;
        protected readonly IValidateService ValidateService;
        protected readonly IReportRecapStockGreigePerTypeService Service;
        protected readonly string ApiVersion;
        public ReportRecapStockGreigePerTypeController(IIdentityService identityService, IValidateService validateService, IReportRecapStockGreigePerTypeService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
        }

        [HttpGet]
        public IActionResult GetReport( DateTime? dateTo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                IdentityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                IdentityService.TimezoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
                IdentityService.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                string accept = Request.Headers["Accept"];

                var data =  Service.GetRecapStocktGreige( dateTo, offset, page, size, Order);



                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Item1,
                    info = new { total = data.Item2 },
                    message = General.OK_MESSAGE,
                    statusCode = General.OK_STATUS_CODE
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("download")]
        public IActionResult GetExcelAll([FromHeader(Name = "x-timezone-offset")] string timezone, DateTime? dateTo)
        {
            try
            {
                //VerifyUser();
                byte[] xlsInBytes;
                int clientTimeZoneOffset = Convert.ToInt32(timezone);
                DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;
                string Tanggal = DateTo.ToString("dd MMM yyyy", new CultureInfo("id-ID"));

                var Result =  Service.GenerateExcel(dateTo, clientTimeZoneOffset);
                string filename = "Saldo Akhir Gudang Grey per Piece- " + Tanggal + ".xlsx";

                xlsInBytes = Result.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
