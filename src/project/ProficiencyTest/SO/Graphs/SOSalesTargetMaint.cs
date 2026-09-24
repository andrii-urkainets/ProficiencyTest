using System;
using System.Globalization;
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

        protected virtual void _(Events.FieldUpdated<SOSalesTarget, SOSalesTarget.typeOfCommand> e)
        {
            if (e.Row == null)
                return;

            e.Cache.SetValue<SOSalesTarget.command>(e.Row, null);
        }

        protected virtual void _(Events.FieldSelecting<SOSalesTarget, SOSalesTarget.command> e)
        {
            if (e.Row == null)
                return;

            bool isManual = e.Row.TypeOfCommand == CommandTypes.Manual;
            bool isSchedule = e.Row.TypeOfCommand == CommandTypes.Schedule;
            object value = e.ReturnValue;

            if (isManual)
            {
                e.ReturnValue = ToBoolean(value);
                e.ReturnState = CreateCommandState(e.ReturnValue, typeof(bool), true);
                return;
            }

            if (isSchedule)
            {
                e.ReturnValue = ToDate(value);
                e.ReturnState = CreateCommandState(e.ReturnValue, typeof(DateTime), true);
                return;
            }

            e.ReturnValue = null;
            e.ReturnState = CreateCommandState(null, typeof(string), false);
        }

        protected virtual void _(Events.FieldUpdating<SOSalesTarget, SOSalesTarget.command> e)
        {
            if (e.Row == null)
                return;

            if (e.Row.TypeOfCommand == CommandTypes.Manual)
            {
                bool? value = ToBoolean(e.NewValue);
                e.NewValue = value == null ? null : value == true ? "1" : "0";
                return;
            }

            if (e.Row.TypeOfCommand == CommandTypes.Schedule)
            {
                DateTime? value = ToDate(e.NewValue);
                e.NewValue = value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                return;
            }

            e.NewValue = null;
        }
        #endregion

        private static PXFieldState CreateCommandState(object value, Type dataType, bool enabled)
        {
            return PXFieldState.CreateInstance(
                value,
                dataType,
                null,
                true,
                null,
                null,
                null,
                null,
                nameof(SOSalesTarget.Command),
                null,
                "Command",
                null,
                PXErrorLevel.Undefined,
                enabled,
                true,
                !enabled,
                PXUIVisibility.Visible,
                null,
                null,
                null);
        }

        private static bool? ToBoolean(object value)
        {
            if (value == null || value is string text && text.Length == 0)
                return null;

            if (value is bool boolean)
                return boolean;

            string stored = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (stored == "1" || string.Equals(stored, "true", StringComparison.OrdinalIgnoreCase))
                return true;
            if (stored == "0" || string.Equals(stored, "false", StringComparison.OrdinalIgnoreCase))
                return false;

            return null;
        }

        private static DateTime? ToDate(object value)
        {
            if (value == null || value is string text && text.Length == 0)
                return null;

            if (value is DateTime date)
                return date.Date;

            if (DateTime.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                return parsed.Date;

            return null;
        }
    }
}