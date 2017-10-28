using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coin_Diffusion
{
    public class City
    {
        public int CoordX { get; private set; }
        public int CoordY { get; private set; }
        public string Country { get; private set; }
        public Dictionary<string, int> Coins { get; set; }

        List<Transaction> Transactions;

        public City(int x, int y, string country)
        {
            CoordX = x;
            CoordY = y;
            this.Country = country;
            Transactions = new List<Transaction>();
            Coins = new Dictionary<string, int>();
        }
		/// <summary>
		/// Linked city with it`s neighbour
		/// </summary>
		/// <param name="Neighbour"></param>
        public void MakeLinkWithNeighbours(City Neighbour)
        {
            Transaction newTransaction = new Transaction();
            newTransaction.receiver = Neighbour;
            newTransaction.values = new Dictionary<string, int>();
            this.Transactions.Add(newTransaction);
            Transaction receiverTransaction = new Transaction();
            receiverTransaction.receiver = this;
            receiverTransaction.values = new Dictionary<string, int>();
            Neighbour.Transactions.Add(receiverTransaction);
        }
		/// <summary>
		/// First filling of each transaction 
		/// </summary>
		/// <param name="type">Type of coin</param>
		/// <param name="value">Value of coin</param>
        public void FillTransaction(string type,int value)
        {
            foreach(var item in Transactions)
            {
               item.values.Add(type, value);
            }
        }
		/// <summary>
		/// Calculate money for transaction
		/// </summary>
        public void PrepareTransaction()
        {
            foreach(var coin in Coins)
            {
                foreach (var transaction in Transactions)
                    transaction.values[coin.Key] = coin.Value / 1000;
            }
        }
		/// <summary>
		/// Execute transactions of each city
		/// </summary>
        public void MakeTransaction()
        {
            foreach(var transaction in Transactions)
            {
                foreach(var type in transaction.values)
                {
                    this.Coins[type.Key] -= type.Value;
                    transaction.receiver.Coins[type.Key] += type.Value;
                }
            }
        }

    }
}
