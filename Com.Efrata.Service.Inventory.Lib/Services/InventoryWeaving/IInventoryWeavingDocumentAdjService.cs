﻿using Com.Efrata.Service.Inventory.Lib.Helpers;
using Com.Efrata.Service.Inventory.Lib.Models.InventoryWeavingModel;
using Com.Efrata.Service.Inventory.Lib.ViewModels.InventoryWeavingViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Efrata.Service.Inventory.Lib.Services.InventoryWeaving
{
    public interface IInventoryWeavingDocumentAdjService
    {
        ListResult<InventoryWeavingDocument> Read(int page, int size, string order, string keyword, string filter);
        List<InventoryWeavingItemDetailViewModel> GetMaterialItemList(string material);
        Task<InventoryWeavingDocument> MapToModel(InventoryWeavingDocumentOutViewModel data);
        Task Create(InventoryWeavingDocument model);
    }
}
