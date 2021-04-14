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

    }
}