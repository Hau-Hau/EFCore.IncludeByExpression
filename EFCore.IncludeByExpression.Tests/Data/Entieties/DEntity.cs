using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.IncludeByExpression.Tests.Data.Entieties
{
    public class DEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DEntity(CEntity parent, BEntity parentAncestor)
        {
            Parent = parent;
            ParentAncestor = parentAncestor;
        }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public CEntity Parent { get; set; }
        public BEntity ParentAncestor { get; set; }
    }
}
