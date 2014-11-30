using System;
using DataSyncBasic2.DataSync;
using XSockets.Core.Utility.Storage;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace DataSyncBasic2
{
    [XSocketMetadata("Animal")]
    public class AnimalController : XSocketsDataSyncController<AnimalController, AnimalModel>
    {
    }
}
