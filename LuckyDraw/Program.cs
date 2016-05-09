using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CsvHelper;
using CsvHelper.Configuration;

namespace LuckyDraw
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader =
                File.OpenText(args[0]);

            var csvConfiguration = new CsvConfiguration();
            csvConfiguration.RegisterClassMap<EntryMap>();

            var csv = new CsvReader(reader, csvConfiguration);

            var records = csv.GetRecords<Entry>()
                .Skip(1)
                .OrderBy(entry => entry.Id, new RandomComparer())
                //.Take(1)
                ;

            foreach (var record in records)
            {
                Console.WriteLine(record.Id);
                //Console.WriteLine($"{record.FirstName} {record.LastName}");
            }

            Console.ReadLine();

        }
    }

    public class RandomComparer : IComparer<int>
    {
        private readonly RNGCryptoServiceProvider _provider;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public RandomComparer()
        {
            _provider = new RNGCryptoServiceProvider();
        }

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(int x, int y)
        {
            byte[] data = new byte[1];
            _provider.GetBytes(data);

            int interim = data[0];
            interim = (interim % 3);
            interim -= 1;

            Debug.WriteLine(interim);

            return interim;
        }
    }

    public class EntryMap : CsvClassMap<Entry>
    {
        public EntryMap()
        {
            Map(m => m.Id).Index(0);
            Map(m => m.FirstName).Index(1);
            Map(m => m.LastName).Index(2);
        }
    }

    public class Entry
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
