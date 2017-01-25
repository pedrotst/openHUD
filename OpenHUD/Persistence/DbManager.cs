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

        public void populateHand(Hand hand)
        {
            string query = "INSERT INTO HAND (Id, TableInfo, PokerType, DealerSeat, Timestamp)" +
                "VALUES (@Id, @TableInfo, @Type, @Dealer, @TS)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", hand.handNumber);
                var tableNameId = populateTableName(hand.tableName, hand.maxSeat, hand.smallBlind, hand.bigBlind, hand.currency);
                command.Parameters.AddWithValue("@TableInfo", tableNameId);
                command.Parameters.AddWithValue("@Type", hand.pokerType);
                command.Parameters.AddWithValue("@Dealer", hand.buttonSeat);
                command.Parameters.AddWithValue("@TS", hand.timestamp);
                command.ExecuteScalar();

                hand.players.ForEach(p => populatePlayer(p, hand.handNumber));
            }
        }

        public void populatePlayer(Player player, double handNumber)
        {
            string query = "INSERT INTO Player (PlayerName, Seat, Hand, Chips, Cards)" +
                "VALUES (@PlayerName, @Seat, @Hand, @Chips, @Cards)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                var playerNameId = populatePlayerName(player.name);
                command.Parameters.AddWithValue("@PlayerName", playerNameId);
                command.Parameters.AddWithValue("@Seat", player.seat);
                command.Parameters.AddWithValue("@Hand", handNumber);
                command.Parameters.AddWithValue("@Chips", player.chips);
                if (player.cards != null)
                {
                    command.Parameters.AddWithValue("@Cards", player.cards);
                }
                else
                {
                    command.Parameters.AddWithValue("@Cards", DBNull.Value);
                }
                command.ExecuteScalar();
            }
        }

        public int populatePlayerName(string name)
        {
            string query = "IF EXISTS (SELECT * FROM PLAYERNAME WHERE Name = @Name)" +
                " SELECT * FROM PLAYERNAME WHERE Name = @Name;" +
                " ELSE BEGIN" +
                " INSERT INTO PlayerName (Name) OUTPUT INSERTED.Id VALUES (@Name); END";
            
            int insertedValue;
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {

                connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                insertedValue = (int)command.ExecuteScalar();
            }
            return insertedValue;
        }

        public int populateTableName(string name, string seatNumber, double smallBlind, double bigBlind, string currency)
        {
            string query = "IF EXISTS (SELECT * FROM TABLEINFO WHERE Name = @Name)" +
                " SELECT * FROM TABLEINFO WHERE Name = @Name;" +
                " ELSE BEGIN" +
                " INSERT INTO TABLEINFO (Name, SeatNumber, SmallBlind, BigBlind, Currency) OUTPUT INSERTED.Id VALUES (@Name, @SeatNumber, @SmallBlind, @BigBlind, @Currency); END";
            
            int insertedValue;
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {

                connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@SeatNumber", seatNumber);
                command.Parameters.AddWithValue("@SmallBlind", smallBlind);
                command.Parameters.AddWithValue("@BigBlind", bigBlind);
                command.Parameters.AddWithValue("@Currency", currency);
                insertedValue = (int)command.ExecuteScalar();
            }
            return insertedValue;
        }

    }
}
