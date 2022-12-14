using Com.Danliris.Service.Inventory.Lib.Helpers;
using Com.Danliris.Service.Inventory.Lib.Models.MaterialsRequestNoteModel;
using Com.Danliris.Service.Inventory.Lib.ViewModels;
using Com.Danliris.Service.Inventory.Test.DataUtils.IntegrationDataUtil;
using Com.Danliris.Service.Inventory.Test.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Danliris.Service.Inventory.Test.DataUtils.MaterialRequestNoteDataUtil
{
    public class MaterialRequestNoteItemDataUtil
    {
        private readonly HttpClientTestService client;

        public MaterialRequestNoteItemDataUtil(HttpClientTestService client)
        {
            this.client = client;
        }

        public MaterialsRequestNote_Item GetNewData()
        {
            ProductionOrderViewModel productionOrder = ProductionOrderDataUtil.GetProductionOrder(client);
            InventorySummaryViewModel product = InventorySummaryDataUtil.GetInventorySummary(client);
            
            return new MaterialsRequestNote_Item
            {
                ProductionOrderId = productionOrder.Id,
                ProductionOrderNo = productionOrder.OrderNo,
                ProductionOrderIsCompleted = false,
                OrderQuantity = (double)productionOrder.OrderQuantity,
                OrderTypeId = productionOrder.OrderType.Id,
                OrderTypeCode = productionOrder.OrderType.Code,
                OrderTypeName = productionOrder.OrderType.Name,
                ProductId = product.productId,
                ProductCode = product.productCode,
                ProductName = product.productName,
                Grade = "A",
                Length = 5
            };
        }

        public MaterialsRequestNote_Item GetNewDataCustom()
        {
            ProductionOrderViewModel productionOrder = ProductionOrderDataUtil.GetProductionOrder(client);
            InventorySummaryViewModel product = InventorySummaryDataUtil.GetInventorySummary(client);

            return new MaterialsRequestNote_Item
            {
                ProductionOrderId = "testCustom",
                ProductionOrderNo = "testCustomNo",
                ProductionOrderIsCompleted = false,
                OrderQuantity = (double)productionOrder.OrderQuantity,
                OrderTypeId = productionOrder.OrderType.Id,
                OrderTypeCode = productionOrder.OrderType.Code,
                OrderTypeName = productionOrder.OrderType.Name,
                ProductId = product.productId,
                ProductCode = product.productCode,
                ProductName = product.productName,
                Grade = "A",
                Length = 5
            };
        }
    }
}
