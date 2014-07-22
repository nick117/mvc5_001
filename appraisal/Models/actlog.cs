namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.actlog")]
    public partial class actlog
    {
        [Display(Name = "紀錄編號")]
        public int id { get; set; }

        [Required]
        [Display(Name = "使用網頁")]
        [StringLength(40)]
        public string App { get; set; }

        [Required]
        [Display(Name = "操作人員")]
        [StringLength(40)]
        public string Pepo { get; set; }

        [Required]
        [Display(Name = "動作說明")]
        [StringLength(200)]
        public string Act { get; set; }

        [Column(TypeName = "text")]
        [Display(Name = "額外說明")]
        [StringLength(65535)]
        public string Ext { get; set; }

        [Display(Name = "紀錄時間")]
        public DateTime Tm { get; set; }
    }
}
