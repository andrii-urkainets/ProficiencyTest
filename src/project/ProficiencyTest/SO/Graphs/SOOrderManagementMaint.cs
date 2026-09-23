using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.SO;

namespace ProficiencyTest
{
    public class SOOrderManagementMaint : PXGraph<SOOrderManagementMaint, SOOrderManagement>
    {
        public SOOrderManagementMaint()
        {
            SOSetup setup = PXSelect<SOSetup>.Select(this);
            SOSetupExt setupExt = setup?.GetExtension<SOSetupExt>();

            if (setupExt == null || setupExt.UsrIncludeOrderManagement != true)
            {
                throw new PXSetupNotEnteredException(
                    ErrorMessages.ErrSalesOrderPrefMissing,
                    typeof(SOSetup),
                    ErrorMessages.SalesOrderPreferencesMessage,
                    PXErrorLevel.Warning
                );
            }
        }

        public SelectFrom<SOOrderManagement>.View Document;

        public SelectFrom<SOOrder>.
            Where<SOOrder.orderType.
                IsEqual<SOOrderManagement.orderType.FromCurrent>.
            And<SOOrder.customerID.
                IsEqual<SOOrderManagement.customerID.FromCurrent>>>.View
            SalesOrders = null!;

        public SelectFrom<SOLine>.
            Where<SOLine.orderType.
                IsEqual<SOOrderManagement.orderType.FromCurrent>.
            And<SOLine.customerID.
                IsEqual<SOOrderManagement.customerID.FromCurrent>>>.View
            SalesOrderLines = null!;

        public SelectFrom<Customer>
            .Where<Customer.bAccountID
                .IsEqual<SOOrderManagement.customerID.FromCurrent>>
            .View Customers;

        public SelectFrom<Address>
            .Where<Address.addressID.IsEqual<
                Customer.defAddressID.FromCurrent>>
            .View CustomerAddresses;

        public SelectFrom<Contact>
            .Where<Contact.contactID.IsEqual<
                Customer.defContactID.FromCurrent>>
            .View CustomerContacts;


        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXCustomize.PXUIFieldAttribute(
            Visible = true, 
            Visibility = PXUIVisibility.SelectorVisible, DisplayName = "Customer ID")]
        protected virtual void _(Events.CacheAttached<Customer.bAccountID> e) { }


        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXCustomize.PXUIFieldAttribute(
            Visible = true,
            Visibility = PXUIVisibility.SelectorVisible, DisplayName = "Account Name")]
                protected virtual void _(Events.CacheAttached<Customer.acctName> e) { }


        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXCustomize.PXUIFieldAttribute(
            Visible = true,
            Visibility = PXUIVisibility.SelectorVisible, DisplayName = "Account ID")]
        protected virtual void _(Events.CacheAttached<Customer.acctCD> e) { }


        protected virtual void _(Events.RowSelected<SOOrder> e)
        {
            if (e.Row == null)
                return;

            SOOrderManagement document = Document.Current;

            if (document == null)
                return;

            Document.Cache.SetValue<SOOrderManagement.currentOrderNbr>(
                document,
                e.Row.OrderNbr
            );
        }
    }
}