using PX.Data;
using PX.Data.BQL;

namespace ProficiencyTest
{
    public class Constants
    {
        public static class CommandTypes
        {
            public const string Manual = "M";
            public const string Schedule = "S";

            public class manual : BqlString.Constant<manual>
            {
                public manual() : base(Manual)
                {
                }
            }

            public class schedule : BqlString.Constant<schedule>
            {
                public schedule() : base(Schedule)
                {
                }
            }
        }
    }
}
