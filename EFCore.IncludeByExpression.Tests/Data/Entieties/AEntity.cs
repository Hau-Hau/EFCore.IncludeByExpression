using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.IncludeByExpression.Tests.Data.Entieties
{
    public class AEntity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public IEnumerable<BEntity>? Childs { get; set; }
    }
}
