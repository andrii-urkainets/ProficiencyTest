using PX.Data;
using PX.Data.BQL;
using PX.Objects.IN;
using System;
using static ProficiencyTest.Constants;

namespace ProficiencyTest
{
    [PXCacheName(Messages.SoSalesTarget)]
    [PXPrimaryGraph(typeof(SOSalesTargetMaint))]
    public class SOSalesTarget : BaseEntity, IBqlTable
    {
        #region SalesTargetID
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Sales Target ID", Visible = false)]
        public virtual int? SalesTargetID { get; set; }
        public abstract class salesTargetID : BqlInt.Field<salesTargetID> { }
        #endregion

        #region InventoryID
        [StockItem()]
        [PXDefault]
        [PXUIField(DisplayName = "Inventory Item", Required = true)]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region SiteID
        [PXDBInt()]
        [PXDefault]
        [PXUIField(DisplayName = "Warehouse", Required = true)]
        [PXSelector(
            typeof(INSite.siteID),
            typeof(INSite.siteCD),
            typeof(INSite.descr),
            SubstituteKey = typeof(INSite.siteCD),
            DescriptionField = typeof(INSite.descr))]
        public virtual int? SiteID { get; set; }
        public abstract class siteID : PX.Data.BQL.BqlInt.Field<siteID> { }
        #endregion

        #region OrderQty
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00")]
        [PXUIField(DisplayName = "Order Qty")]
        public virtual Decimal? OrderQty { get; set; }
        public abstract class orderQty : PX.Data.BQL.BqlDecimal.Field<orderQty> { }
        #endregion

        #region UnitPrice
        [PXDBDecimal(2)]
        [PXDefault(TypeCode.Decimal, "0.00")]
        [PXUIField(DisplayName = "Unit Price")]
        public virtual Decimal? UnitPrice { get; set; }
        public abstract class unitPrice : PX.Data.BQL.BqlDecimal.Field<unitPrice> { }
        #endregion

        #region TypeOfCommand
        public abstract class typeOfCommand : BqlString.Field<typeOfCommand> { }

        [PXDBString(1, IsFixed = true, IsUnicode = false)]
        [PXDefault(CommandTypes.Manual, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Type Of Command")]
        [PXStringList(
            new[]
            {
                CommandTypes.Manual,
                CommandTypes.Schedule
            },
            new[]
            {
                "Manual",
                "Schedule"
            })]
        public virtual string TypeOfCommand { get; set; }
        #endregion

        #region Command
        public abstract class command : BqlString.Field<command> { }

        [PXDBString(30, IsUnicode = true)]
        [PXUIField(DisplayName = "Command")]
        public virtual string Command { get; set; }
        #endregion
    }
}