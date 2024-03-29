﻿using Com.Efrata.Service.Inventory.Lib.Services;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Report.Expenditure.Aval;
using Com.Efrata.Service.Inventory.WebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Efrata.Service.Inventory.WebApi.Controllers.v1.GarmentLeftoverWarehouse.ReportExpenditure
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/garment/leftover-warehouse-expenditure/monitoring-avals")]
    [Authorize]
    public class ExpenditureAvalMonitoringController : Controller
    {
        private IIdentityService IdentityService;
        private readonly IValidateService ValidateService;
        private readonly IExpenditureAvalMonitoringService Service;
        private readonly string ApiVersion;

        public ExpenditureAvalMonitoringController(IIdentityService identityService, IValidateService validateService, IExpenditureAvalMonitoringService service)
        {
            IdentityService = identityService;
            ValidateService = validateService;
            Service = service;
            ApiVersion = "1.0.0";
        }

        protected void VerifyUser()
        {
            IdentityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            IdentityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            IdentityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpGet]
        public IActionResult Get(DateTime? dateFrom, DateTime? dateTo, string type, int page, int size, string Order = "{}")
        {
            int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
            string accept = Request.Headers["Accept"];

            try
            {
                VerifyUser();

                var data = Service.GetMonitoring(dateFrom, dateTo, type, page, size, Order, offset);

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    data = data.Item1,
                    info = new { total = data.Item2 }
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
        public IActionResult GetXlsAll(DateTime? dateFrom, DateTime? dateTo, string type)
        {

            try
            {
                VerifyUser();
                byte[] xlsInBytes;
                int offset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
                DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : Convert.ToDateTime(dateFrom);
                DateTime DateTo = dateTo == null ? DateTime.Now : Convert.ToDateTime(dateTo);

                var generatedExcel = Service.GenerateExcel(dateFrom, dateTo, type, offset);

                string filename = String.Format("Report Pengeluaran Gudang Sisa Aval- {0}.xlsx", DateTime.UtcNow.ToString("ddMMyyyy"));

                return File(generatedExcel.Item1.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", generatedExcel.Item2);


            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
