using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Helper.OneData
{
    class OnePageSize
    {
        private OneAutomatic autoMatice;
        public OnePageSize(XElement XElem)
        {
            autoMatice = new OneAutomatic(XElem);
        }
    }
}
