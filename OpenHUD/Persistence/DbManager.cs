using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace OpenHud.Persistence
{
    class DbManager
    {
        SqlConnection connection;
        string connectionString;

        public DbManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["HandHistoryCS"].ConnectionString;
        }

        public void populateHands(Hand hand)
        {
            string query = "INSERT INTO HAND (Id, TableName, SmallBlind, BigBlind, Currency, PokerType, SeatNumber, DealerSeat)" +
                "VALUES (@Id, @Tname, @BigBlind, @SmallBlind, @Currency, @Type, @Num, @Dealer)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", hand.handNumber);
                command.Parameters.AddWithValue("@Tname", hand.tableName);
                command.Parameters.AddWithValue("@BigBlind", hand.smallBlind);
                command.Parameters.AddWithValue("@SmallBlind", hand.bigBlind);
                command.Parameters.AddWithValue("@Currency", hand.currency);
                command.Parameters.AddWithValue("@Type", hand.pokerType);
                command.Parameters.AddWithValue("@Num", hand.maxSeat);
                command.Parameters.AddWithValue("@Dealer", hand.buttonSeat);

                command.ExecuteScalar();
            }
        }
    }
}
