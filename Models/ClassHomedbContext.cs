using ClassHome.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClassHome.Models
{
    public class ClassHomedbContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
    {
        public DbSet<UserModel> Useres { get; set; }
        public DbSet<TurmaModel> Turmas { get; set; }
        public DbSet<DisciplinaModel> Disciplinas { get; set; }/* 
        public DbSet<ProfessorModel> Professores { get; set; } */
        public DbSet<AlunoModel> Alunos { get; set; }
        public DbSet<ProfessorDisciplinaModel> ProfessorDisciplina { get; set; } 
        public DbSet<TurmaUserModel> TurmaUser { get; set; } 
        public DbSet<MatriculaModel> Matriculas { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        public ClassHomedbContext(DbContextOptions<ClassHomedbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>().ToTable("User");

            modelBuilder.Entity<TurmaModel>().ToTable("Turma");
            modelBuilder.Entity<DisciplinaModel>().ToTable("Disciplina");/* 
            modelBuilder.Entity<ProfessorModel>().ToTable("Professor"); */
            modelBuilder.Entity<AlunoModel>().ToTable("Aluno");
            modelBuilder.Entity<ProfessorDisciplinaModel>().ToTable("ProfessorDisciplina"); 
            modelBuilder.Entity<TurmaUserModel>().ToTable("TurmaUser"); 

            modelBuilder.Entity<Publicacao>().ToTable("Publicacao");
            modelBuilder.Entity<Publicacao>().Property(e => e.DataPublicacao).HasDefaultValueSql("datetime('now')")
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<Comentario>().ToTable("Comentario");
            modelBuilder.Entity<Comentario>().Property(c => c.DataComentario).HasDefaultValueSql("datetime('now')")
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<ProfessorDisciplinaModel>()
                .HasKey(p => new { p.ProfessorId , p.DisciplinaId });
 
            modelBuilder.Entity<TurmaUserModel>()
                .HasKey(t => new { t.UserId , t.TurmaId }); 
        }
    }
}