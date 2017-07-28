﻿using System;

namespace BIZ.PControl.DomainModel.CalcBalance
{
    public class Balance
    {
        public virtual DateTime Date { get; set; }
        public virtual Product Product { get; set; }
        public virtual double Quantity { get; set; }

        public override bool Equals(object obj)
        {
            return Date == ((Balance)obj).Date && Product.Id == ((Balance)obj).Product.Id;
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode() + Product.GetHashCode();
        }
    }
}