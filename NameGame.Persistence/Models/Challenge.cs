using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NameGame.Persistence.Models
{
    public class Challenge
    {
        public Challenge(int userId, string correctAnswer)
        {
            UserId = userId;
            CorrectAnswer = correctAnswer;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }

        public int Id { get; set; }

        public int Attempts { get; set; }

        public int UserId { get; set; }

        public bool Solved { get; set; }

        public string CorrectAnswer { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
