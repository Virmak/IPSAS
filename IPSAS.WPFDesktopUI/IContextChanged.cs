using System;
using System.Collections.Generic;
using System.Text;

namespace IPSAS.WPFDesktopUI
{
    public interface IContextChanged
    {
        public void RefreshContext(object o = null);
    }
}
