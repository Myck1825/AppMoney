using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppModey.Database.Constants
{
    public static class ApplicationStoredProcedure
    {
        public const string GetApplicationByID = "sp_GetApplicationByID_Procedure";
        public const string GetApplicationByCliendIdAndDepartementAddress = "sp_GetApplicationByCliendIdAndDepartementAddress_Procedure";
        public const string RegisterApplication = "sp_RegisterApplication_Procedure";
    }
}
