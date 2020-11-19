using HomeTask.Models;
using HomeTask.ViewModels;

namespace HomeTask.Controllers.UtilityControllers
{
    public class UtilityStorage
    {
        public static Storage Create(StorageView storageView)
        {
            var storage = new Storage
            {
                StorageName = storageView.StorageName,
                Address = storageView.Address,
            };
            return storage;
        }
    }
}