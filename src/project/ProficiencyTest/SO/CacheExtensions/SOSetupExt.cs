using PX.Data;
using PX.Objects.SO;

namespace ProficiencyTest
{
    public sealed class SOSetupExt : PXCacheExtension<SOSetup>
    {
        #region UsrIncludeOrderManagement

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Include Order Management")]
        public bool? UsrIncludeOrderManagement { get; set; }

        public abstract class usrIncludeOrderManagement : PX.Data.BQL.BqlBool.Field<usrIncludeOrderManagement>
        {
        }

        #endregion

        #region UsrIncludeSalesTarget

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Include Sales Target")]
        public bool? UsrIncludeSalesTarget { get; set; }

        public abstract class usrIncludeSalesTarget : PX.Data.BQL.BqlBool.Field<usrIncludeOrderManagement>
        {
        }

        #endregion
    }
}
