using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Coin_Diffusion
{
    class Program
    {
        struct Country : IComparable
        {
            public string Name;
            public int x1, x2, y1, y2;
            public int CompareTo(object obj)
            {
                Country second = (Country)obj;
                if (this.y1 < second.y1)
                    return -1;
                else
                {
                    if (this.y1 > second.y1)
                        return 1;
                    else
                    {
						return (this.x1 < second.x1) ? -1 : 1;
                    }
                }
            }
        }

            
        static void Write(int count, Dictionary<string,int> results)
        {
            Console.WriteLine("Case Number {0}",count);
            foreach(var country in results)
            {
                Console.WriteLine("{0} {1}",country.Key,country.Value);
            }
        }

        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("testCases.txt");
            int count = 0;
			int hashRatio = 10;
            while (!reader.EndOfStream)
            {
                List<City> cities = new List<City>();
                Hashtable table = new Hashtable();
                int c;
                bool result = Int32.TryParse(reader.ReadLine(), out c);
                if (result)
                {
                    if (c == 0)
                        break;
                    Country[] countries = new Country[c];

                    for (int i = 0; i < c; i++)
                    {
                        try
                        {
                            int[] coords = new int[4];
                            string[] buf = reader.ReadLine().Split(new char[] { ' ' });
                            countries[i].Name = buf[0];
                            countries[i].x1 = Convert.ToInt32(buf[1]);
                            countries[i].y1 = Convert.ToInt32(buf[2]);
                            countries[i].x2 = Convert.ToInt32(buf[3]);
                            countries[i].y2 = Convert.ToInt32(buf[4]);
                            if (countries[i].x1 <= 0 || countries[i].y1 <= 0 ||
                                countries[i].x2 <= 0 || countries[i].y2 <= 0)
                            {
                                Console.WriteLine("Incorrect data, requires all coordinates biger then 0");
                                goto incorrect_data;
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Incorrect data\n"+e.Message);
                            goto incorrect_data;
                        }
                    }
                    if (c > 1)
                    {
                        Array.Sort(countries);
                        string[] names = new string[c];
                        int h = 0;
                        foreach (var item in countries)
                        {
                            names[h] = item.Name;
                            h++;
                            for (int k = item.x1; k <= item.x2; k++)
                                for (int j = item.y1; j <= item.y2; j++)
                                {
                                    int xNeighbour = (k - 1) * hashRatio + j;
                                    int yNeighbour = k * hashRatio + j - 1;
                                    var city = new City(k, j, item.Name);
                                    cities.Add(city);
                                    table.Add(k * hashRatio + j, city);
                                    if (table.Contains(yNeighbour))
                                        city.MakeLinkWithNeighbours(table[yNeighbour] as City);
                                    if (table.Contains(xNeighbour))
                                        city.MakeLinkWithNeighbours(table[xNeighbour] as City);
                                }
                        }

                        Transition transition = new Transition(cities, names);
                        Write(++count, transition.CountDuration());
                    }
                    else
                        Write(++count, new Dictionary<string, int>() { { countries[0].Name, 0 } });
                }
                else
                {
                    Console.WriteLine("Incorrect data");
                    break;
                }
            incorrect_data:;
            }
            
            //Console.Read();
        }
    }
}
