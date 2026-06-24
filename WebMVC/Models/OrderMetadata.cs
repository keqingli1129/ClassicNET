namespace WebMVC.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    public class OrderMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> OrderDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> RequiredDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ShippedDate { get; set; }
    }
}
