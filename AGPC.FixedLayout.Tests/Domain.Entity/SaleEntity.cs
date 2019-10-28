using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.Domain.Entity
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }

    public  class SaleEntity : Entity
    {

        private SaleEntity() { }
        private SaleEntity(string salename, DateTime startdate, Decimal estimatedtotalsale) 
        {
            this.Id = Guid.NewGuid();
            this.SaleName = salename;
            this.StartDate = startdate;
            this.EstimatedTotalSale = estimatedtotalsale;
        }

        public static SaleEntity Create(string salename,DateTime startdate,Decimal estimatedtotalsale) =>
            new SaleEntity(salename, startdate, estimatedtotalsale);
        

        public string SaleName { get; set; }

        public DateTime StartDate { get; set; }

        public Decimal EstimatedTotalSale { get; set; }
    }
}
