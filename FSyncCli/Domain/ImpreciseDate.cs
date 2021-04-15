using System;

namespace FSyncCli.Domain
{
    public struct ImpreciseDate
    {
        public ImpreciseDate(int year, int? month, int? day) : this()
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; private set; }
        public int? Month { get; private set; }
        public int? Day { get; private set; }

        public ImpreciseDatePrecision Precision
        {
            get
            {
                if (Day.HasValue) return ImpreciseDatePrecision.Day;

                if (Month.HasValue) return ImpreciseDatePrecision.Month;

                return ImpreciseDatePrecision.Year;
            }
        }

        public override string ToString()
        {
            switch (Precision)
            {
                case ImpreciseDatePrecision.Year:
                    return $"{Year:D4}";
                case ImpreciseDatePrecision.Month:
                    return $"{Year:D4}.{Month:D2}";
                case ImpreciseDatePrecision.Day:
                    return $"{Year:D4}.{Month:D2}.{Day:D2}";
                default:
                    return $"{Year:D4}.{Month:D2}.{Day:D2}";
            }
        }
    }
}