using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.models
{
    public class Article
    {

        [Key]
        public int Id{set;get;}

        [Required(ErrorMessage ="lỗi nhập {0}")]
        [StringLength(255, MinimumLength =5, ErrorMessage ="Yêu cầu nhập từ {2} đến {1}")]
        [Column(TypeName = "nvarchar")]
        [DisplayName("Tiêu Đề")]
        public string? Title{set;get;}

        [DataType(DataType.Date)]
        [Required(ErrorMessage ="lỗi nhập {0}")]
        [DisplayName("Ngày Tạo")]
        public DateTime Created{set;get;}

        [Column(TypeName ="ntext")]
        [DisplayName("Nội Dung")]
        public string? Content{set;get;}
    }}
    
