using Microsoft.AspNetCore.Mvc;

namespace HomeTask.ViewModels
{
    public class StorageView
    {
        [Remote("VerifyStorageName", "Validator", ErrorMessage = "Storage already exist")]
        public string StorageName { get; set; }
        public string Address { get; set; }
    }
}