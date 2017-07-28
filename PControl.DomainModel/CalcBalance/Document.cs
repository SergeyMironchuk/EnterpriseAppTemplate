using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BIZ.PControl.DomainModel.CalcBalance
{
    public class Document
    {
        public virtual long Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Description { get; set; }
        private ISet<DocumentDetail> _details;

        public virtual ISet<DocumentDetail> Details
        {
            get
            {
                _details = _details ?? new HashSet<DocumentDetail>();
                return _details;
            }
            set { _details = value; }
        }

        public virtual void AddDetail(DocumentDetail detail)
        {
            detail.Document = this;
            Details.Add(detail);
        }

        public virtual string AdditionalInformation { get; set; }
    }
}