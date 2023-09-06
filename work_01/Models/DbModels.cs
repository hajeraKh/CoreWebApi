using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.Models
{
    public class Skill
    {
        public Skill()
        {
            this.CandidateSkills = new List<CandidateSkill>();
        }
        public int SkillId { get; set; }
        public string? SkillName { get; set; }
        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; }
    }
    public class Candidate
    {
        public Candidate()
        {
            this.CandidateSkills = new List<CandidateSkill>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = default!;
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = default!;
        public string Picture { get; set; } = default!;
        public bool Fresher { get; set; }
        public string? Password { get; set; }
        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; }

    }
    public class CandidateSkill
    {
        public int CandidateSkillId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        //if
        public string? SkillName { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Skill Skill { get; set; }
    }
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext(DbContextOptions<CandidateDbContext> options) : base(options) { }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData
            (
                new Skill { SkillId = 1, SkillName = "C#" },
                new Skill { SkillId = 2, SkillName = "SQL" },
                new Skill { SkillId = 3, SkillName = "HTML" },
                new Skill { SkillId = 4, SkillName = "Java" },
                new Skill { SkillId = 5, SkillName = "PHP" }
            );
            base.OnModelCreating(modelBuilder);
        }
        internal object Find(int CandidateId)
        {
            throw new NotImplementedException();
        }
    }
}
