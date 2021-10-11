using System;

namespace SimpleBalance.Models
{
    public class Flow
    {
        public Flow(string id, string sourceId, string destinationId, string name, double? measured, double? tolerance,
                    bool? isMeasured, bool? isExcluded, double? lowerBound, double? upperBound)
        {
            Id = id;
            SourceId = sourceId;
            DestinationId = destinationId;
            Name = name ?? throw new ArgumentNullException(nameof(name));//означает проверку на null. Если переменная name = null - подставляется вторая(exception).
            Measured = measured ?? throw new ArgumentNullException(nameof(measured));
            Tolerance = tolerance ?? throw new ArgumentNullException(nameof(tolerance));
            IsMeasured = isMeasured ?? throw new ArgumentNullException(nameof(isMeasured));
            IsExcluded = isExcluded ?? throw new ArgumentNullException(nameof(isExcluded));
            LowerBound = lowerBound ?? throw new ArgumentNullException(nameof(lowerBound));
            UpperBound = upperBound ?? throw new ArgumentNullException(nameof(upperBound));
        }

        public string Id { get; set; }
        public string SourceId { get; set; }
        public string DestinationId { get; set; }
        public string Name { get; set; }

        public double Measured { get; set; }
        public double Tolerance { get; set; }

        public bool IsMeasured { get; set; }
        public bool IsExcluded { get; set; }

        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
    }
}
