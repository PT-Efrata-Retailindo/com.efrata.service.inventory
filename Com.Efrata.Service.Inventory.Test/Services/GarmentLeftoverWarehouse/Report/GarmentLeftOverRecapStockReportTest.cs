﻿using Com.Efrata.Service.Inventory.Lib;
using Com.Efrata.Service.Inventory.Lib.Models.GarmentLeftoverWarehouse.Stock;
using Com.Efrata.Service.Inventory.Lib.Services;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.BalanceStock;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.ExpenditureAccessories;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.ExpenditureFabric;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.ExpenditureFinishedGood;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFabricServices;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFinishedGoodServices;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.ReceiptAccessories;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Report.Bookkeeping;
using Com.Efrata.Service.Inventory.Lib.Services.GarmentLeftoverWarehouse.Stock;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.BalanceStock;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.ExpenditureAccessories;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.ExpenditureFabric;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.ExpenditureFinishedGood;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFabricDataUtils;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.GarmentLeftoverWarehouseReceiptFinishedGoodDataUtils;
using Com.Efrata.Service.Inventory.Test.DataUtils.GarmentLeftoverWarehouse.ReceiptAccessories;
using Com.Efrata.Service.Inventory.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Efrata.Service.Inventory.Test.Services.GarmentLeftoverWarehouse.Report
{
    public class GarmentLeftOverRecapStockReportTest
    {
        private const string ENTITY = "recapStockReport";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private InventoryDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<InventoryDbContext> optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            InventoryDbContext dbContext = new InventoryDbContext(optionsBuilder.Options);

            return dbContext;
        }
        private GarmentLeftoverWarehouseReceiptFabricDataUtil _dataUtilReceiptFabric(GarmentLeftoverWarehouseReceiptFabricService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseReceiptFabricDataUtil(service);
        }
        private GarmentLeftoverWarehouseExpenditureFabricDataUtil _dataUtilFabric(GarmentLeftoverWarehouseExpenditureFabricService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseExpenditureFabricDataUtil(service);
        }
        private GarmentLeftoverWarehouseReceiptAccessoriesDataUtil _dataUtilReceiptAcc(GarmentLeftoverWarehouseReceiptAccessoriesService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseReceiptAccessoriesDataUtil(service);
        }
        private GarmentLeftoverWarehouseExpenditureAccessoriesDataUtil _dataUtilAcc(GarmentLeftoverWarehouseExpenditureAccessoriesService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseExpenditureAccessoriesDataUtil(service);
        }
        private GarmentLeftoverWarehouseExpenditureFinishedGoodDataUtil _dataUtilFinishedGood(GarmentLeftoverWarehouseExpenditureFinishedGoodService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseExpenditureFinishedGoodDataUtil(service);
        }
        private GarmentLeftoverWarehouseReceiptFinishedGoodDataUtil _dataUtilReceiptFinishedGood(GarmentLeftoverWarehouseReceiptFinishedGoodService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseReceiptFinishedGoodDataUtil(service);
        }
        private GarmentLeftoverWarehouseBalanceStockDataUtil _dataUtilbalanceStock(GarmentLeftoverWarehouseBalanceStockService service)
        {

            GetServiceProvider();
            return new GarmentLeftoverWarehouseBalanceStockDataUtil(service);
        }
        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            return serviceProvider;
        }

        private Mock<IServiceProvider> GetFailServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpFailTestService());



            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });


            return serviceProvider;
        }
        [Fact]
        public async Task Should_Success_GetRecapStockReportTypeFabric()
        {


            var serviceProvider = GetServiceProvider();

            var stockServiceMock = new Mock<IGarmentLeftoverWarehouseStockService>();
            stockServiceMock.Setup(s => s.StockOut(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(1);

            stockServiceMock.Setup(s => s.StockIn(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(1);

            serviceProvider
                .Setup(x => x.GetService(typeof(IGarmentLeftoverWarehouseStockService)))
                .Returns(stockServiceMock.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpTestService());

            GarmentLeftoverWarehouseRecapStockReportService utilService = new GarmentLeftoverWarehouseRecapStockReportService(_dbContext(GetCurrentMethod()), GetServiceProvider().Object);

            GarmentLeftoverWarehouseExpenditureFabricService service = new GarmentLeftoverWarehouseExpenditureFabricService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseBalanceStockService _balanceservice = new GarmentLeftoverWarehouseBalanceStockService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseReceiptFabricService receiptFabservice = new GarmentLeftoverWarehouseReceiptFabricService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            var dataFabric = await _dataUtilFabric(service).GetTestData();

            var data_Balance = _dataUtilbalanceStock(_balanceservice).GetTestData_FABRIC();

            var dataReceiptAcc = _dataUtilReceiptFabric(receiptFabservice).GetTestData();

            var result = utilService.GetReportQuery( DateTime.Now, DateTime.Now,7);


            Assert.NotNull(result);
        }
        [Fact]
        public async Task Should_Success_GetFlowStockReport()
        {
            var serviceProvider = GetServiceProvider();

            var stockServiceMock = new Mock<IGarmentLeftoverWarehouseStockService>();
            stockServiceMock.Setup(s => s.StockOut(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(1);
            stockServiceMock.Setup(s => s.StockIn(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(1);


            serviceProvider
                .Setup(x => x.GetService(typeof(IGarmentLeftoverWarehouseStockService)))
                .Returns(stockServiceMock.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpTestService());

            GarmentLeftoverWarehouseRecapStockReportService utilService = new GarmentLeftoverWarehouseRecapStockReportService(_dbContext(GetCurrentMethod()), GetServiceProvider().Object);

            GarmentLeftoverWarehouseExpenditureAccessoriesService service = new GarmentLeftoverWarehouseExpenditureAccessoriesService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseBalanceStockService _balanceservice = new GarmentLeftoverWarehouseBalanceStockService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseReceiptAccessoriesService receiptAccservice = new GarmentLeftoverWarehouseReceiptAccessoriesService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            var dataFabric = await _dataUtilAcc(service).GetTestData();

            var data_Balance = _dataUtilbalanceStock(_balanceservice).GetTestData_FINISHEDGOOD();

            var dataReceiptAcc = _dataUtilReceiptAcc(receiptAccservice).GetTestData();
            var result = utilService.GetReportQuery( DateTime.Now, DateTime.Now, 7);


            Assert.NotNull(result);
        }
        [Fact]
        public async Task Should_Success_GetFlowStockExcelReport()
        {
            var serviceProvider = GetServiceProvider();

            var stockServiceMock = new Mock<IGarmentLeftoverWarehouseStockService>();
            stockServiceMock.Setup(s => s.StockOut(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(1);
            stockServiceMock.Setup(s => s.StockIn(It.IsAny<GarmentLeftoverWarehouseStock>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
              .ReturnsAsync(1);


            serviceProvider
                .Setup(x => x.GetService(typeof(IGarmentLeftoverWarehouseStockService)))
                .Returns(stockServiceMock.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpService)))
                .Returns(new HttpTestService());

            GarmentLeftoverWarehouseRecapStockReportService utilService = new GarmentLeftoverWarehouseRecapStockReportService(_dbContext(GetCurrentMethod()), GetServiceProvider().Object);

            GarmentLeftoverWarehouseExpenditureAccessoriesService service = new GarmentLeftoverWarehouseExpenditureAccessoriesService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseBalanceStockService _balanceservice = new GarmentLeftoverWarehouseBalanceStockService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            GarmentLeftoverWarehouseReceiptAccessoriesService receiptAccservice = new GarmentLeftoverWarehouseReceiptAccessoriesService(_dbContext(GetCurrentMethod()), serviceProvider.Object);

            var dataFabric = await _dataUtilAcc(service).GetTestData();

            var data_Balance = _dataUtilbalanceStock(_balanceservice).GetTestData_FINISHEDGOOD();

            var dataReceiptAcc = _dataUtilReceiptAcc(receiptAccservice).GetTestData();
            var result = utilService.GenerateExcel(DateTime.Now, DateTime.Now, 7);


            Assert.NotNull(result);
        }


    }
}

