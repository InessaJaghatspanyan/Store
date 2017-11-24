using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Data_Access
{
    public enum ProductUniqueEnum
    {
        success = 1,
        codeNotUnique,
        nameNotUnique,
        barcodeNotUnique
    }
}