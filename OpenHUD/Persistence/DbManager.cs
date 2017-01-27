using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using OpenHud.Model;

namespace OpenHud.Persistence
{
    class DbManager
    {
        SqlConnection _connection;
        internal string ConnectionString;

        public DbManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["HandHistoryCS"].ConnectionString;
        }

        public void PopulateHand(Hand hand)
        {
            var query = "INSERT INTO HAND (Id, TableInfo, DealerSeat, Board, Timestamp)" +
                "VALUES (@Id, @TableInfo, @Dealer, @Board, @Timestamp)";

            using (_connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                command.Parameters.AddWithValue("@Id", hand.HandNumber);
                var tableNameId = PopulateTableName(hand.TableName, hand.MaxSeat, hand.SmallBlind, 
                    hand.BigBlind, hand.Currency, hand.PokerType);
                command.Parameters.AddWithValue("@TableInfo", tableNameId);
                command.Parameters.AddWithValue("@Dealer", hand.ButtonSeat);
                command.Parameters.AddWithValue("@Timestamp", hand.Timestamp);
                command.Parameters.AddWithValue("@Board", (Object) hand.Board ?? DBNull.Value);
                command.ExecuteScalar();

                hand.Players.ForEach(p => PopulatePlayer(p, hand.HandNumber));
            }
        }

        public void PopulatePlayer(Player player, long handNumber)
        {
            var query = "INSERT INTO Player (PlayerName, Seat, Hand, Chips, Cards) " +
                "OUTPUT INSERTED.Id " + 
                "VALUES (@PlayerName, @Seat, @Hand, @Chips, @Cards)";

            using (_connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                var playerNameId = PopulatePlayerName(player.Name);
                command.Parameters.AddWithValue("@PlayerName", playerNameId);
                command.Parameters.AddWithValue("@Seat", player.Seat);
                command.Parameters.AddWithValue("@Hand", handNumber);
                command.Parameters.AddWithValue("@Chips", player.Chips);
                command.Parameters.AddWithValue("@Cards", (Object) player.Cards ?? DBNull.Value);
                long playerId = (long)command.ExecuteScalar();
                player.Actions.ForEach(act => PopulateAction(playerId, handNumber, act));
            }
        }

        private void PopulateAction(long playerId, long handId, PlayerAction playerAction)
        {
            var query = "INSERT INTO Action (Hand, Player, Name, Value, Stage, ActionNum)" +
                "VALUES (@Hand, @PlayerName, @ActionName, @Value, @Stage, @ActionNum)";

            using (_connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                command.Parameters.AddWithValue("@Hand", handId);
                command.Parameters.AddWithValue("@PlayerName", playerId);
                command.Parameters.AddWithValue("@ActionName", playerAction.actionName);
                command.Parameters.AddWithValue("@Value", (Object)playerAction.value ?? DBNull.Value);
                command.Parameters.AddWithValue("@Stage", playerAction.stage);
                command.Parameters.AddWithValue("@ActionNum", playerAction.actionNumber);
                command.ExecuteScalar();
            }
        }

        public int PopulatePlayerName(string name)
        {
            // Only adds if its a new Name, otherwise fetch its id
            const string query = "IF EXISTS (SELECT * FROM PLAYERNAME WHERE Name = @Name)" +
                                 " SELECT * FROM PLAYERNAME WHERE Name = @Name;" +
                                 " ELSE BEGIN" +
                                 " INSERT INTO PlayerName (Name) OUTPUT INSERTED.Id VALUES (@Name); END";
            
            int insertedValue;
            using (_connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, _connection))
            {

                _connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                insertedValue = (int)command.ExecuteScalar();
            }
            return insertedValue;
        }

        public long PopulateTableName(string name, string seatNumber, double smallBlind, double bigBlind, 
            string currency, string pokerType)
        {
            // Only adds if its a new table, otherwise fetch its id
            const string query = "IF EXISTS (SELECT * FROM TABLEINFO WHERE Name = @Name)" +
                                 " SELECT * FROM TABLEINFO WHERE Name = @Name;" +
                                 " ELSE BEGIN" +
                                 " INSERT INTO TABLEINFO (Name, SeatNumber, SmallBlind, BigBlind, Currency, PokerType) OUTPUT INSERTED.Id" +
                                 " VALUES (@Name, @SeatNumber, @SmallBlind, @BigBlind, @Currency, @PokerType);" +
                                 " END";
            
            long insertedValue;
            using (_connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(query, _connection))
            {

                _connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@SeatNumber", seatNumber);
                command.Parameters.AddWithValue("@SmallBlind", smallBlind);
                command.Parameters.AddWithValue("@BigBlind", bigBlind);
                command.Parameters.AddWithValue("@Currency", currency);
                command.Parameters.AddWithValue("@PokerType", pokerType);
                insertedValue = (long)command.ExecuteScalar();
            }
            return insertedValue;
        }

    }
}
