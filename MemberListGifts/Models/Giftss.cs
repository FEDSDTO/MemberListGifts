//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MemberListGifts.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Giftss
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Giftss()
        {
            this.UsedRule = new HashSet<UsedRule>();
        }
    
        public int Id { get; set; }
        public string Year { get; set; }
        public string MallId { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string GiftsNo { get; set; }
        public Nullable<int> StartNo { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Denomination { get; set; }
        public string Type { get; set; }
        public string MallType { get; set; }
        public Nullable<bool> IsPlus { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<int> ModifyUser { get; set; }
        public Nullable<System.DateTime> ModifyTime { get; set; }
        public Nullable<bool> IsUse { get; set; }
        public string CaseNumber { get; set; }
        public string SAPType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsedRule> UsedRule { get; set; }
    }
}
