namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.emp")]
    public partial class emp
    {
        public emp()
        {
            ts1 = new HashSet<ts>();
            ts2 = new HashSet<ts>();
        }
        [Display(Name = "�Ǹ�")]
        public int id { get; set; }

        [Required]
        [Display(Name = "�d��")]
        [StringLength(12)]
        public string eid { get; set; }

        [Required]
        [Display(Name = "�m�W")]
        [StringLength(20)]
        public string cname { get; set; }

        [Display(Name = "�����s��")]
        public int dept { get; set; }

        [Required]
        [Display(Name = "¾��")]
        [StringLength(40)]
        public string title { get; set; }

         [ForeignKey("dept")]
        public virtual dep dep { get; set; }

        [ForeignKey("emp")]
        public virtual ICollection<ts> ts1 { get; set; }

        [ForeignKey("boss")]
        public virtual ICollection<ts> ts2 { get; set; }

    }
}
