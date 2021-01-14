using System;
using CsvHelper;
using Nest;

namespace ElasticsearchCSVLoadExample
{
    public class Helpers
    {
        public static ElasticClient CreateElasticClient(string host)
        {
            Uri node = new Uri(host);

            ConnectionSettings settings = new ConnectionSettings(node);

            return new ElasticClient(settings);
        }

        public static HospitalReadmission ParseCsvRecordToHospitalReadmission(CsvReader csv)
        {
            // Retrieve the data from the row we know we can safely extract
            HospitalReadmission record = new HospitalReadmission()
            {
                FacilityName = csv.GetField<string>("Facility Name"),
                FacilityId = csv.GetField<int>("Facility ID"),
                State = csv.GetField<string>("State"),
                MeasureName = csv.GetField<string>("Measure Name"),
                Footnote = csv.GetField<string?>("Footnote"),
                StartDate = csv.GetField<DateTime>("Start Date"),
                EndDate = csv.GetField<DateTime>("End Date"),
                IndexedAt = DateTime.Now,
            };

            /*
             * Note: Some of the number values have "N/A" if
             * there is no data present, so to safely parse this
             * value as an int/decimal we will use the TryParse()
             * method and fallback to leaving the field null if
             * the parsing fails.
             */

            if (int.TryParse(csv.GetField<string>("Number of Discharges"), out int numberOfDischarges))
            {
                record.NumberOfDischarges = numberOfDischarges;
            }

            if (decimal.TryParse(csv.GetField<string>("Excess Readmission Ratio"), out decimal excessReadmissionRatio))
            {
                record.ExcessReadmissionRatio = excessReadmissionRatio;
            }

            if (decimal.TryParse(csv.GetField<string>("Predicted Readmission Rate"), out decimal predictedReadmissionRate))
            {
                record.PredictedReadmissionRate = predictedReadmissionRate;
            }

            if (decimal.TryParse(csv.GetField<string>("Expected Readmission Rate"), out decimal expectedReadmissionRate))
            {
                record.ExpectedReadmissionRate = expectedReadmissionRate;
            }

            if (int.TryParse(csv.GetField<string>("Number of Readmissions"), out int numberOfReadmissions))
            {
                record.NumberOfReadmissions = numberOfReadmissions;
            }

            return record;
        }
    }
}
