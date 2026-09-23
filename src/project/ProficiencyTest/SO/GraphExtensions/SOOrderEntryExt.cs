using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;

namespace ProficiencyTest
{
    public class SOOrderEntry_Extension : PXGraphExtension<SOOrderEntry>
    {
        public PXSetup<SOSetup> Setup;

        protected virtual void _(Events.FieldUpdated<SOOrder, SOOrder.customerID> e)
        {
            if (e.Row == null)
                return;

            SOOrder row = e.Row;

            SOSetup setup = SelectFrom<SOSetup>.View.Select(Base);

            if (setup == null)
                return;

            SOSetupExt setupExt = setup.GetExtension<SOSetupExt>();

            if (setupExt?.UsrIncludeOrderManagement != true)
                return;

            if (string.IsNullOrEmpty(row.OrderType) || row.CustomerID == null)
                return;

            SOOrderManagement orderManagement = SelectFrom<SOOrderManagement>
                    .Where<SOOrderManagement.orderType.IsEqual<@P.AsString>
                        .And<SOOrderManagement.customerID
                            .IsEqual<@P.AsInt>>>.View
                    .Select(Base, row.OrderType, row.CustomerID);

            if (orderManagement == null)
                return;

            e.Cache.SetValue<SOOrder.orderDesc>(
                row,
                orderManagement.Description);
        }

        protected virtual void _(Events.FieldUpdated<SOLine, SOLine.inventoryID> e)
        {
            if (e.Row == null)
                return;

            SetSalesTargetPriceAndQuantity(e.Row);
        }

        protected virtual void _(Events.FieldUpdated<SOLine, SOLine.siteID> e)
        {
            if (e.Row == null)
                return;

            SetSalesTargetPriceAndQuantity(e.Row);
        }

        private void SetSalesTargetPriceAndQuantity(SOLine line)
        {
            if (line.InventoryID == null || line.SiteID == null)
                return;

            SOSetupExt setupExt = Setup.Current?.GetExtension<SOSetupExt>();

            if (setupExt?.UsrIncludeSalesTarget != true)
                return;

            SOSalesTarget target =
                SelectFrom<SOSalesTarget>
                    .Where<SOSalesTarget.inventoryID.IsEqual<@P.AsInt>
                        .And<SOSalesTarget.siteID.IsEqual<@P.AsInt>>>.View
                    .Select(Base, line.InventoryID, line.SiteID);

            if (target == null)
                return;

            if (line.UnitPrice != target.UnitPrice)
            {
                Base.Transactions.Cache.SetValueExt<SOLine.curyUnitPrice>(
                    line,
                    target.UnitPrice);
            }

            if (line.OrderQty != target.OrderQty)
            {
                Base.Transactions.Cache.SetValueExt<SOLine.orderQty>(
                    line,
                    target.OrderQty);
            }
        }
    }
}