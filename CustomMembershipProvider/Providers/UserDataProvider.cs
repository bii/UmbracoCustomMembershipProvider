using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace CustomMembershipProvider.Providers
{
    public class UserDataProvider
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;

        public CmsMember FindByProviderUserKey(string providerUserKey)
        {
            string sql = "SELECT id FROM umbracoNode WHERE uniqueId = @uniqueId";
            
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                // ReSharper disable once PossibleNullReferenceException
                var nodeId = dbConnection.Query<UmbracoNode>(sql, new {UniqueId = providerUserKey})
                    .SingleOrDefault().Id;
                sql = "SELECT * FROM cmsMember WHERE NodeId = @NodeId";
                
                return dbConnection.Query<CmsMember>(sql, new {NodeId = nodeId}).SingleOrDefault();
            }
        }
        
        public CmsMember Find(string loginName)
        {
            string sql = "SELECT * FROM cmsMember WHERE LoginName = @loginName";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                return dbConnection.Query<CmsMember>(sql, new {loginName}).SingleOrDefault();
            }
        }
        
        public CmsMember FindByEmail(string email)
        {
            string sql = "SELECT * FROM cmsMember WHERE Email = @Email";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                return dbConnection.Query<CmsMember>(sql, new {Email = email}).SingleOrDefault();
            }
        }
        
        public IEnumerable<CmsMember> FindAll()
        {
            string sql = "SELECT * FROM cmsMember";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                return dbConnection.Query<CmsMember>(sql);
            }
        }

        public Dictionary<string, string> GetMemberProperties(string loginName, out string name, out string providerUserKey)
        {
            string sql =
                "SELECT		    un.uniqueId AS ProviderUserKey, ucv.[text] AS Name, cpt.Alias, ISNULL(CONVERT(NVARCHAR(MAX), upd.intValue), ISNULL(CONVERT(NVARCHAR(MAX),upd.dateValue), ISNULL(CONVERT(NVARCHAR(MAX), upd.varcharValue) ,CONVERT(NVARCHAR(MAX), upd.textValue)))) AS VAL" +
                " FROM			cmsMember				cm" +
                " INNER JOIN	umbracoContentVersion	ucv" +
                " ON cm.nodeId		= ucv.nodeId" +
                " INNER JOIN	umbracoPropertyData		upd" +
                " ON ucv.id			= upd.versionId" +
                " INNER JOIN	cmsPropertyType			cpt" +
                " ON cpt.id			= upd.propertyTypeId" +
                " INNER JOIN	umbracoNode				un" +
                " ON un.[id]			=	ucv.nodeId" +
                " WHERE	cm.LoginName	= @LoginName";

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                IEnumerable<MemberProperty> memberProperties = 
                    dbConnection.Query<MemberProperty>(sql, new {LoginName = loginName});
                var properties = memberProperties as MemberProperty[] ?? memberProperties.ToArray();
                name = properties.FirstOrDefault()?.Name;
                providerUserKey = properties.FirstOrDefault()?.ProviderUserKey.ToString("N");
                
                return properties.ToDictionary(property => property.Alias, property => property.Val);
            }
        }

    }
}