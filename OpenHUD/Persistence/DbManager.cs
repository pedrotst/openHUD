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

                hand.players.ForEach(p => populatePlayer(p, hand.handNumber));
            }
        }

        public void populatePlayer(Player player, double handNumber)
        {
            string query = "INSERT INTO Player (Name, Seat, Hand, Chips, Card1Rank, Card1Suit, Card2Rank, Card2Suit)" +
                "VALUES (@Name, @Seat, @Hand, @Chips, @Card1Rank, @Card1Suit, @Card2Rank, @Card2Suit)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", player.name);
                command.Parameters.AddWithValue("@Seat", player.seat);
                command.Parameters.AddWithValue("@Hand", handNumber);
                command.Parameters.AddWithValue("@Chips", player.chips);
                if (player.cards != null)
                {
                    command.Parameters.AddWithValue("@Card1Rank", player.cards[0].card.Item1);
                    command.Parameters.AddWithValue("@Card1Suit", player.cards[0].card.Item2);
                    command.Parameters.AddWithValue("@Card2Rank", player.cards[1].card.Item1);
                    command.Parameters.AddWithValue("@Card2Suit", player.cards[1].card.Item2);
                }
                else
                {
                    command.Parameters.AddWithValue("@Card1Rank", DBNull.Value);
                    command.Parameters.AddWithValue("@Card1Suit", DBNull.Value);
                    command.Parameters.AddWithValue("@Card2Rank", DBNull.Value);
                    command.Parameters.AddWithValue("@Card2Suit", DBNull.Value);
                }
                command.ExecuteScalar();
            }
        }

    }
}
