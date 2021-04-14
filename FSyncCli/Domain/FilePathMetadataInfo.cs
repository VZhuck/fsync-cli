using System;

namespace FSyncCli.Domain
{
    public class FilePathMetadataInfo
    {
        public ImpreciseDate? From { get; set; }

        public ImpreciseDate? To { get; set; }

        public string CategoryName{ get; set; }
        
        public string CategorySubPath { get; set; }

        protected bool Equals(FilePathMetadataInfo other)
        {
            return Nullable.Equals(From, other.From) && Nullable.Equals(To, other.To) && CategoryName == other.CategoryName && CategorySubPath == other.CategorySubPath;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FilePathMetadataInfo)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To, CategoryName, CategorySubPath);
        }

    }
}