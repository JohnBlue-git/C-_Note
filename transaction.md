The transaction is a set of operations treated as a unit, and either all take effect or none. SQL Server uses a transaction log to ensure that each transaction is completed successfully and securely.

- The transaction log records all changes made by a transaction, including inserts, updates, deletes, and any other operations.
- Each time a change is made to the database, an entry is written to the transaction log with information about the operation.
- This information includes the time the operation was made, the user who executed it, the type of operation that occurred (insert, update, delete), and any modified data.
- When a transaction is started in SQL Server, all changes to the database are recorded in an "uncommitted" state until the user explicitly commits them.
- It allows users to roll back any changes if needed, as all of the data is still available in its original state.
- Once a transaction has been committed, it is written to the database and can no longer be rolled back or undone.

c#的transection跟dapper

https://stackoverflow.com/questions/44118876/which-transaction-is-better-with-dapper-begin-tran-or-transactionscope

c# transaction

https://learn.microsoft.com/zh-tw/aspnet/web-forms/overview/data-access/working-with-batched-data/wrapping-database-modifications-within-a-transaction-cs

```csharp
// Create the SqlTransaction object
SqlTransaction myTransaction = SqlConnectionObject.BeginTransaction();
try
{
    /*
     * ... Perform the database transaction�s data modification statements...
     */
    // If we reach here, no errors, so commit the transaction
    myTransaction.Commit();
}
catch
{
    // If we reach here, there was an error, so rollback the transaction
    myTransaction.Rollback();
    throw;
}

public void UpdateWidgetQuantity(int widgetId, int quantity)
{
    using(var conn = new SqlConnection("{connection string}")) {
        conn.Open();

        // create the transaction
        // You could use `var` instead of `SqlTransaction`
        using(SqlTransaction tran = conn.BeginTransaction()) {
            try
            {
                var sql = "update Widget set Quantity = @quantity where WidgetId = @id";
                var parameters = new { id = widgetId, quantity };

                // pass the transaction along to the Query, Execute, or the related Async methods.
                conn.Execute(sql, parameters, tran);

                // if it was successful, commit the transaction
                tran.Commit();
            }
            catch(Exception ex)
            {
                // roll the transaction back
                tran.Rollback();

                // handle the error however you need to.
                throw;
            }
        }
    }   
}

```
