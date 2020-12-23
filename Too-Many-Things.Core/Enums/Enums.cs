using System;
using System.Collections.Generic;
using System.Text;

namespace Too_Many_Things.Core.Enums
{
    public static class Enums
    {
        public enum SortOrder
        {
            Default,
            AscendingOrder,
            DescendingOrder
        }

        public enum AuthenticationType
        {
            SqlServerAuthentication,
            MSServerAuthentication
        }

        public enum InterfaceState
        {
            Default,
            Renaming,
            Deleting
        }
    }
}
