using PX.Common;

namespace ProficiencyTest
{
    [PXLocalizable()]
    public static class ErrorMessages
    {
        public const string DuplicateSalesTarget = "For this quantity InventoryItem and Warehouse already exists.";

        public const string ErrSalesOrderPrefMissing = "The requested resource is not available. The required configuration data is not entered on the Sales Orders Preferences form.";

        public const string SalesOrderPreferencesMessage =
            "Navigate to the [Sales Orders Preferences] form and enter the required configuration data.";

        public const string OrderManagementRecordAlreadyExists =
            "An Order Management record already exists for this Order Type and Customer.";
    }

    public static class Messages
    {
        //DAC names
        public const string SoSalesTarget = "SO Sales Target";
        public const string SoOrderManagement = "Order Management";
        public const string SoOrderInfo = "SO Order Info";
        public const string DetailInfo = "Detail Info";
    }
}
