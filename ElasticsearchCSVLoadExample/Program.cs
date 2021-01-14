using System;
using Nest;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace ElasticsearchCSVLoadExample
{
    class Program
    {
        static async Task Main()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            string IndexName = config["IndexName"];
            string ElasticsearchHostUrl = config["ElasticsearchHostUrl"];
            string CsvDataPath = config["CsvDataPath"];

            Console.Out.WriteLine("Connecting to ES");

            // Instantiate an ElasticClient with correct connection info and options
            ElasticClient client = Helpers.CreateElasticClient(ElasticsearchHostUrl);

            Console.Out.WriteLine("Opening CSV");

            // Create a read stream from CSV with data
            using StreamReader reader = new StreamReader(CsvDataPath);
            using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            Console.Out.WriteLine("Starting read");

            // Start reading and load in the headers
            csv.Read();
            csv.ReadHeader();

            List<HospitalReadmission> documents = new List<HospitalReadmission>();
            int documentCount = 0;

            // Read and process all rows until no more are available
            while (csv.Read())
            {
                HospitalReadmission document = Helpers.ParseCsvRecordToHospitalReadmission(csv);

                documentCount += 1;
                documents.Add(document);

                if (documentCount >= 500)
                {
                    Console.Out.WriteLine("Writing batch");

                    await client.BulkAsync(b => b
                        .Index(IndexName)
                        .IndexMany(documents)
                    );

                    documentCount = 0;
                    documents.Clear();
                }
            }

            Console.Out.WriteLine("Finishing");

            await client.BulkAsync(b => b
                .Index(IndexName)
                .IndexMany(documents)
            );

            Console.Out.WriteLine("Done");
        }
    }
}
