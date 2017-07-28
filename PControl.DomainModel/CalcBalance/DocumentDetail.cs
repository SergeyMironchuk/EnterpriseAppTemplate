namespace BIZ.PControl.DomainModel.CalcBalance
{
    public class DocumentDetail
    {
        public virtual long Id { get; set; }
        public virtual Document Document { get; set; }
        public virtual Product Product { get; set; }
        public virtual double Quantity { get; set; }
    }
}