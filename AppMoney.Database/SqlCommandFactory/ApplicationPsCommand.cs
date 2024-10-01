namespace AppMoney.Database.SqlCommandFactory
{
    public class ApplicationPsCommand : IApplicationSqlCommand
    {
        public string RegisterApplicationStoredProcedureName => "\"sp_RegisterApplication_Procedure\"";

        public string GetByFilter() =>
            "SELECT * FROM sp_getapplicationbycliendidanddepartementaddress_procedure(@client_id, @department_address);";

        public string GetById() => "SELECT * FROM sp_getapplicationbyid_procedure(@appId);";
    }
}
