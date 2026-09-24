using PX.Data;
using PX.Data.BQL;
using PX.Objects.AR;
using PX.Objects.SO;

namespace ProficiencyTest
{
    [PXCacheName(Messages.SoOrderManagement)]
    [PXPrimaryGraph(typeof(SOOrderManagementMaint))]
    public class SOOrderManagement : BaseEntity, IBqlTable
    {
        #region OrderManagementID
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Order Management ID", Visible = false)]
        public virtual int? OrderManagementID { get; set; }
        public abstract class orderManagementID : BqlInt.Field<orderManagementID> { }
        #endregion

        #region OrderType

        public abstract class orderType : BqlString.Field<orderType> { }

        [PXDBString(2, IsUnicode = false, IsFixed = true, InputMask = ">aa")]
        [PXDefault]
        [PXUIField(DisplayName = "Order Type")]
        [PXSelector(
            typeof(Search<SOOrderType.orderType>),
            typeof(SOOrderType.orderType),
            typeof(SOOrderType.descr)
        )]
        public virtual string OrderType { get; set; }

        #endregion

        #region CustomerID

        public abstract class customerID : BqlInt.Field<customerID> { }

        [PXDBInt]
        [PXDefault]
        [PXUIField(DisplayName = "Customer")]
        [PXSelector(
            typeof(Search<Customer.bAccountID>),
            typeof(Customer.bAccountID),
            typeof(Customer.acctName),
            typeof(Customer.acctCD),
            SubstituteKey = typeof(Customer.acctCD),
            DescriptionField = typeof(Customer.acctName))]
        public virtual int? CustomerID { get; set; }

        #endregion

        #region Description
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "Description", Required = false)]
        public virtual string Description { get; set; }
        public abstract class description :
                PX.Data.BQL.BqlString.Field<description>
        { }
        #endregion

        #region CurrentOrderNbr

        public abstract class currentOrderNbr : BqlString.Field<currentOrderNbr>
        {
        }

        [PXString(15, IsUnicode = false)]
        [PXUIField(DisplayName = "Current Order Nbr.", Enabled = false)]
        [PXUnboundDefault]
        public virtual string CurrentOrderNbr { get; set; }

        #endregion
    }
}