using Microsoft.Data.SqlClient;
using System.Data;

namespace BulkInsert
{
    public class DatabaseOperations
    {
        public bool InsertBulkRecord<T>(List<T> tickets)
        {
            bool inserted = false;
            DataTable dataTable = GetTable(tickets);

            using(SqlConnection connection = GetConnection())
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection);
                sqlBulkCopy.DestinationTableName = "Tickets";
                connection.Open();
                sqlBulkCopy.WriteToServer(dataTable);
                inserted = true;
            }
            dataTable.Rows.Clear();
            return inserted;
        }


        private SqlConnection GetConnection()
        {
            return new SqlConnection("Data Source=thelottofactorydb.database.windows.net;Database=thelottofactory-dev-db;User ID=ky; Password=L0tt0F@ct0ry");
        }
        public DataTable GetTable<T>(List<T> tickets)
        {
            DataTable table = new DataTable();
            var type = typeof(T);

            //Add rows
            for(int i=0; i<tickets.Count(); i++)
            {
                table.Rows.Add(table.NewRow());
            }

            foreach(var prop in type.GetProperties())
            {
                DataColumn column = new DataColumn(prop.Name);
                column.DataType = prop.PropertyType;
                table.Columns.Add(column);

                //Add values in each cell of the table

                int rowIndex = 0;

                foreach (var item in tickets)
                {
                    DataRow dr = table.Rows[rowIndex++];
                    dr[prop.Name] = prop.GetValue(item);
                }
            }

            return table;
        }
    }
}
