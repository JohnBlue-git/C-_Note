
using
```c#
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using System.Data.SqlClient;
using Dapper;
using SqlBulkTools;
```

dapper
```csharp
Type t = typeof(FutureData);
Console.WriteLine("The {0} type has the following properties: ", t.Name);
foreach (var prop in t.GetProperties())
    Console.WriteLine("   {0} ({1})", prop.Name,
                      prop.PropertyType.Name);
var n = t.GetProperties().Select(x => x.Name);
可以產生所需column

sql = $"SELECT DISTINCT {...column...} FROM {...table...} WHERE {...}";
param = _updateData.Select(x => new {
                        TransferDate = x.TransferDate,
                        DataSource = x.DataSource,
                        UnderlyingCode = x.UnderlyingCode,
                        ProductMonth = x.ProductMonth,
                        PX_LAST = x.PX_LAST,
                        CreateDate = DateTime.Now
                    });

public bool DapperQuery<T>(string connStr, string sql, object? param, out IEnumerable<T> data)
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            data = conn.Query<T>(sql, param);
            if (data == null) 不對
            {
                Console.WriteLine($"[INFO] dapper sql query nothing ...");
                return false;
            }
        }
    }
    catch (Exception e)
    {
        data = null;
        Console.WriteLine("[ERROR] dapper sql query fail\n");
        Console.WriteLine("[ERROR] message: {0}\n", e.ToString());
    }
    return data == null;
}

_updateBLPSql = "UPDATE [BLP_FuturesPrice]"
              + " SET PX_LAST=@PX_LAST, CreateDate=@CreateDate"
              + " WHERE TransferDate=@TransferDate AND DataSource=@DataSource AND UnderlyingCode=@UnderlyingCode AND ProductMonth=@ProductMonth";

public bool DapperExecute<T>(string connStr, string sql, object? param, IEnumerable<T> data)
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            bool check = conn.Execute<T>(sql, param);
            if (check == false)
            {
                Console.WriteLine($"[ERROR] dapper sql execute fail ...");
                return false;
            }
        }
    }
    catch (Exception e)
    {
        data = null;
        Console.WriteLine("[ERROR] dapper sql execute error\n");
        Console.WriteLine("[ERROR] message: {0}\n", e.ToString());
    }
    return data == null;
}

### dapper bulkinsert (money money)
https://www.learndapper.com/bulk-operations/bulk-insert

### sql bulk tool
```c#
bulk.Setup<T>()
    .ForCollection(IEnumerable<T> data)
    .WithTable(string table)
    .AddAllColumns()
    .BulkInsertOrUpdate()
    .MatchTargetOn(x => x.TransferDate)
    .MatchTargetOn(x => x.DataSource)
    .MatchTargetOn(x => x.UnderlyingCode)
    .MatchTargetOn(x => x.ProductMonth);
    // 但是在這 Commit
		或是
		.ForObject(T oneItem)
    .WithTable(string table)
    .AddColumn(x => x.Symbol)
		...

public bool BulkUpdateInsert(string connStr, string table, BulkOperations bulk)
{
    try
    {
        using (TransactionScope trans = new TransactionScope())
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                bulk.Commit(conn);
            }
            trans.Complete();
        }
        return true;
    }
    catch (Exception e)
    {
        Console.WriteLine("[ERROR] bulk sql execute error\n");
        Console.WriteLine("[ERROR] message: {0}\n", e.ToString());
        return false;
    }
}
```
