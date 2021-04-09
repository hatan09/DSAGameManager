using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSAGameDBManager.Core.Entities
{
    [Table("feedbacks")]
    public class Feedback
    {
        public Guid Feedbak_id { get; set; }

        public string Topic { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "Creation date is required")]
        public DateTime Date_created { get; set; }

        [ForeignKey(nameof(player))]
        public string Player_id { get; set; }
        public Player player { get; set; }
    }
}
