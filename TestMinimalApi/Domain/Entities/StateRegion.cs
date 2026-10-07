using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMinimalApi.Domain.Entities
{
    public class StateRegion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string Code { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? NameMM { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? GLISRefCode { get; set; }
        [Column(TypeName = "varchar(1)")]
        public string? SystemUse { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedUser { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedIPAddr { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedUser { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedIPAddr { get; set; }
    }
}
