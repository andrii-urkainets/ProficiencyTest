using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.SO;
using static ProficiencyTest.Constants;

namespace ProficiencyTest
{
    public class SOSalesTargetMaint : PXGraph<SOSalesTargetMaint, SOSalesTarget>
    {
        public SOSalesTargetMaint()
        {
            SOSetup setup = PXSelect<SOSetup>.Select(this);
            SOSetupExt setupExt = setup?.GetExtension<SOSetupExt>();

            if (setupExt == null || setupExt.UsrIncludeSalesTarget != true)
            {
                throw new PXSetupNotEnteredException(
                    ErrorMessages.ErrSalesOrderPrefMissing,
                    typeof(SOSetup),
                    ErrorMessages.SalesOrderPreferencesMessage,
                    PXErrorLevel.Warning
                );
            }
        }

        #region Data Views
        public SelectFrom<SOSalesTarget>.View Targets = null!;

        #endregion

        #region Events
        protected virtual void _(Events.RowPersisting<SOSalesTarget> e)
        {
            if (e.Row == null)
                return;

            if (e.Operation == PXDBOperation.Delete)
                return;

            SOSalesTarget row = e.Row;

            if (row.InventoryID == null || row.SiteID == null)
                return;

            if (e.Operation == PXDBOperation.Update)
            {
                SOSalesTarget original = (SOSalesTarget)e.Cache
                    .GetOriginal(e.Row);

                if (original != null &&
                    original.InventoryID == row.InventoryID &&
                    original.SiteID == row.SiteID)
                {
                    return;
                }
            }

            SOSalesTarget existing = SelectFrom<SOSalesTarget>
                .Where<SOSalesTarget.inventoryID.IsEqual<@P.AsInt>
                    .And<SOSalesTarget.siteID.IsEqual<@P.AsInt>>>
                .View.ReadOnly.Select(
                    this,
                    row.InventoryID,
                    row.SiteID);

            if (existing == null)
                return;

            throw new PXRowPersistingException(
                typeof(SOSalesTarget.orderQty).Name,
                row.OrderQty,
                ErrorMessages.DuplicateSalesTarget);
        }

        //protected virtual void _(Events.RowSelected<SOSalesTarget> e)
        //{
        //    if (e.Row == null)
        //        return;

        //    bool isManual =
        //        e.Row.TypeOfCommand == CommandTypes.Manual;

        //    bool isSchedule =
        //        e.Row.TypeOfCommand == CommandTypes.Schedule;

        //    PXUIFieldAttribute.SetVisible<SOSalesTarget.manualCommand>(
        //        e.Cache,
        //        e.Row,
        //        isManual);

        //    PXUIFieldAttribute.SetVisible<SOSalesTarget.scheduledCommand>(
        //        e.Cache,
        //        e.Row,
        //        isSchedule);

        //    PXUIFieldAttribute.SetEnabled<SOSalesTarget.manualCommand>(
        //        e.Cache,
        //        e.Row,
        //        isManual);

        //    PXUIFieldAttribute.SetEnabled<SOSalesTarget.scheduledCommand>(
        //        e.Cache,
        //        e.Row,
        //        isSchedule);
        //}

        //protected virtual void _(Events.FieldUpdated<SOSalesTarget, SOSalesTarget.typeOfCommand> e)
        //{
        //    if (e.Row == null)
        //        return;

        //    if (e.Row.TypeOfCommand == CommandTypes.Manual)
        //    {
        //        e.Row.ScheduledCommand = null;
        //    }
        //    else if (e.Row.TypeOfCommand == CommandTypes.Schedule)
        //    {
        //        e.Row.ManualCommand = false;
        //    }
        //}
        #endregion
    }
}