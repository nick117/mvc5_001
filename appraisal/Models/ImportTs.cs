namespace appraisal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("d2014.ImportTs")]
    public partial class ImportTs
    {
            [Key]
            [Display(Name = "卡號")]
            [StringLength(5)]
            public string CardNo { get; set; }

            [Display(Name = "姓名")]
            [StringLength(45)]
            public string Name { get; set; }

            [Display(Name = "部門")]
            [StringLength(45)]
            public string Depart { get; set; }

            [Display(Name = "組別")]
            [StringLength(45)]
            public string Group { get; set; }

            [Display(Name = "管理職稱")]
            [StringLength(45)]
            public string MTitle { get; set; }

            [Display(Name = "專業職稱")]
            [StringLength(45)]
            public string PTitle { get; set; }

            [Display(Name = "個人")]
            [StringLength(45)]
            public string Personal { get; set; }

            [Display(Name = "今年曾任單位1")]
            [StringLength(45)]
            public string Job1 { get; set; }

            [Display(Name = "今年曾任單位2")]
            [StringLength(45)]
            public string Job2 { get; set; }

            [Display(Name = "考核等級")]
            [StringLength(4)]
            public string Type { get; set; }

            [Display(Name = "初評人員卡號")]
            [StringLength(45)]
            public string CardNo1 { get; set; }

            [Display(Name = "初評人員")]
            [StringLength(45)]
            public string Name1 { get; set; }

            [Display(Name = "複評人員卡號")]
            [StringLength(45)]
            public string CardNo2 { get; set; }

            [Display(Name = "複評人員")]
            [StringLength(45)]
            public string Name2 { get; set; }
    }
}