//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pickup_point
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pickup_point()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int id { get; set; }
        public Nullable<int> index { get; set; }
        public Nullable<int> city_id { get; set; }
        public Nullable<int> street_id { get; set; }
        public string number { get; set; }
    
        public virtual City City { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
        public virtual Street Street { get; set; }
    }
}
