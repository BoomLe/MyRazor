using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFWebRazor.models
{
    public class Article
    {

        [Key]
        public int Id{set;get;}

        [Required]
        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string? Title{set;get;}

        [DataType(DataType.Date)]
        [Required]
        public DateTime Created{set;get;}

        [Column(TypeName ="ntext")]
        public string? Content{set;get;}
    }}
    
