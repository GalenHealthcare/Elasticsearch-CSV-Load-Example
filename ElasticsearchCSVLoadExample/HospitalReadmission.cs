using System;

namespace ElasticsearchCSVLoadExample
{
    public class HospitalReadmission
    {
        public string FacilityName { get; set; } = null!;
        public int FacilityId { get; set; }
        public string State { get; set; } = null!;
        public string MeasureName { get; set; } = null!;
        public int? NumberOfDischarges { get; set; }
        public string? Footnote { get; set; }
        public decimal? ExcessReadmissionRatio { get; set; }
        public decimal? PredictedReadmissionRate { get; set; }
        public decimal? ExpectedReadmissionRate { get; set; }
        public int? NumberOfReadmissions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime IndexedAt { get; set; }
    }
}
