using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMoney.Database.SqlCommandFactory
{
    public class ApplicationMsCommand : IApplicationSqlCommand
    {
        public string RegisterApplicationStoredProcedureName => "sp_RegisterApplication_Procedure";

        public string GetByFilter() =>
            "EXEC sp_GetApplicationByCliendIdAndDepartementAddress_Procedure @client_id, @department_address;";



        public string GetById() => "EXEC sp_GetApplicationByID_Procedure @appId;";
    }
}
